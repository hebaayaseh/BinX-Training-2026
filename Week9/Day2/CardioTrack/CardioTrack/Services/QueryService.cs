using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IQuery;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services
{
    public class QueryService : IQuery
    {
        private readonly CardioTrackDbContext dbContext;
        public QueryService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<PaginatedPatientsResponseDto> GetPatientsAsync(int userId, GetPatientsQueryRequestDto query)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.Role == UserRole.Doctor
                                     && u.IsActive);
            if (doctor == null)
                throw new InvalidTokenException("Auth forbidden");

            var patientsQuery = dbContext.patients
                .Where(p => p.DoctorId == doctor.Id);

            if (query.Gender.HasValue)
                patientsQuery = patientsQuery.Where(p => p.Gender == query.Gender.Value);

            if (query.BloodType.HasValue)
                patientsQuery = patientsQuery.Where(p => p.BloodType == query.BloodType.Value);

            var totalCount = await patientsQuery.CountAsync();

            patientsQuery = query.SortBy?.ToLower() switch
            {
                "dateofbirth" => query.SortDescending
                    ? patientsQuery.OrderByDescending(p => p.DateOfBirth)
                    : patientsQuery.OrderBy(p => p.DateOfBirth),
                _ => query.SortDescending
                    ? patientsQuery.OrderByDescending(p => p.FullName)
                    : patientsQuery.OrderBy(p => p.FullName)
            };

            var patients = await patientsQuery
                .Skip((query.PageNumber - 1) * query.PageSize)
                .Take(query.PageSize)
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

            return new PaginatedPatientsResponseDto
            {
                Patients = patients,
                TotalCount = totalCount,
                PageNumber = query.PageNumber,
                PageSize = query.PageSize,
                TotalPages = (int)Math.Ceiling(totalCount / (double)query.PageSize)
            };
        }
    }
}
