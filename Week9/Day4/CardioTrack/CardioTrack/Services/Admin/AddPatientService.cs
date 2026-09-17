using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Interfaces.ICache;
using CardioTrack.Models;
using CardioTrack.Services.AuditLogservice;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class AddPatientService : IAddPatient
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IAuditLog log;
        private readonly IPatientCacheInvalidator cacheInvalidator;
        public AddPatientService(CardioTrackDbContext dbContext , IAuditLog log, IPatientCacheInvalidator cacheInvalidator)
        {
            this.dbContext = dbContext;
            this.log = log;
            this.cacheInvalidator = cacheInvalidator;
        }

        public async Task<string> AddPatientAsync(int userId, AddPatientRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(a => a.Id == userId
                                    && a.IsActive
                                    && a.Role == UserRole.Admin);

            if (user == null)
                throw new ForbiddenException("Auth Forbidden");

            var doctor = await dbContext.users
                .FirstOrDefaultAsync(d => d.Id == request.DoctorId
                                     && d.IsActive
                                     && d.Role == UserRole.Doctor);

            if (doctor == null)
                throw new BadRequestException("Doctor not found");

            var patient = new Models.Patient
            {
                FullName = request.FullName,
                PhoneNumber = request.PhoneNumber,
                Address = request.Address,
                BloodType = request.BloodType,
                DateOfBirth = request.DateOfBirth,
                DoctorId = request.DoctorId,
                Gender = request.Gender
            };
            await dbContext.AddAsync(patient);

            await log.LogAsync("AddPatient", "Patient", patient.Id, null, patient);
            await dbContext.SaveChangesAsync();
            await cacheInvalidator.InvalidateAsync(request.DoctorId);

            return "تم تسجيل المريض بنجاح";
        }
    }
}
