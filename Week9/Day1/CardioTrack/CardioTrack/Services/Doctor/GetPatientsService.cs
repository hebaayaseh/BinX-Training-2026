using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Distributed;
using System.Text.Json;

namespace CardioTrack.Services.Doctor
{
    public class GetPatientsService : IGetPatients
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IDistributedCache cache;

        public GetPatientsService(CardioTrackDbContext dbContext, IDistributedCache cache)
        {
            this.dbContext = dbContext;
            this.cache = cache;
        }

        public async Task<PatientsDto> GetPatientAsync(int userId, GetPatientRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && (u.Role == UserRole.Doctor
                                     || u.Role == UserRole.Nurse)
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth forbidden");

            var patient = await dbContext.patients
                    .FirstOrDefaultAsync(p => p.Id == request.PatientId);
            if (patient == null)
                throw new BadRequestException("Patient not found");


            if (user.Role == UserRole.Doctor && patient.DoctorId != userId)
                throw new ForbiddenException("Doctors can only manage their own appointments");


            return new PatientsDto
            {
                PatientId = patient.Id,
                PhoneNumber = patient.PhoneNumber,
                Gender = patient.Gender,
                BloodType = patient.BloodType,
                Address = patient.Address,
                FullName = patient.FullName,
                DateOfBirth = patient.DateOfBirth,

            };


        }

        public async Task<PaginatedPatientsResponseDto> GetPatientsAsync(int userId, GetPatientsQueryRequestDto query)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId && u.Role == UserRole.Doctor && u.IsActive);
            if (doctor == null)
                throw new InvalidTokenException("Auth forbidden");
            var stopwatch = System.Diagnostics.Stopwatch.StartNew();
            string cacheKey = $"patients:doctor:{doctor.Id}";

            var cachedData = await cache.GetStringAsync(cacheKey);
            List<PatientsDto> allPatients;

            if (cachedData != null)
            {
                allPatients = JsonSerializer.Deserialize<List<PatientsDto>>(cachedData)!;
            }
            else
            {
                allPatients = await dbContext.patients
                    .Where(p => p.DoctorId == doctor.Id)
                    .Select(d => new PatientsDto
                    {
                        PatientId = d.Id,
                        PhoneNumber = d.PhoneNumber,
                        Gender = d.Gender,
                        BloodType = d.BloodType,
                        Address = d.Address,
                        FullName = d.FullName,
                        DateOfBirth = d.DateOfBirth,
                    })
                    .ToListAsync();

                var cacheOptions = new DistributedCacheEntryOptions
                {
                    AbsoluteExpirationRelativeToNow = TimeSpan.FromMinutes(5)
                };
                await cache.SetStringAsync(cacheKey, JsonSerializer.Serialize(allPatients), cacheOptions);
            }

            var filtered = allPatients.AsEnumerable();

            if (query.Gender.HasValue)
                filtered = filtered.Where(p => p.Gender == query.Gender.Value);
            if (query.BloodType.HasValue)
                filtered = filtered.Where(p => p.BloodType == query.BloodType.Value);

            filtered = query.SortBy?.ToLower() switch
            {
                "dateofbirth" => query.SortDescending
                    ? filtered.OrderByDescending(p => p.DateOfBirth)
                    : filtered.OrderBy(p => p.DateOfBirth),
                _ => query.SortDescending
                    ? filtered.OrderByDescending(p => p.FullName)
                    : filtered.OrderBy(p => p.FullName)
            };

            var totalCount = filtered.Count();
            var pagedResult = filtered
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
                .ToList();

            stopwatch.Stop();
            Console.WriteLine($"Request took: {stopwatch.ElapsedMilliseconds}ms");
            return new PaginatedPatientsResponseDto
            {
                Patients = pagedResult,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }
    }
}
