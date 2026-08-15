using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Interfaces.IEmail;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Doctor
{
    public class ActiveOrDeactivePatientService : IActiveDeactivePatient
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IEmail email;
        public ActiveOrDeactivePatientService(CardioTrackDbContext dbContext,IEmail email)
        {
            this.dbContext = dbContext;
            this.email = email;
        }
        public async Task<string> ActivePatientProfile(int userId, ActivePatientProfileRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u=>u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Doctor);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId
                                     && p.DoctorId == userId
                                     && p.LinkedUserId!=null);

            if(patient == null)
                throw new ForbiddenException("Auth forbidden");

            var password = GenerateTempPassword();

            var user = new User
            {
                Email = request.Email,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(password),
                PhoneNumber = patient.PhoneNumber,
                CreatedAt = DateTime.UtcNow,
                UpdatedAt = DateTime.UtcNow,
                IsActive = true,
                Role = UserRole.Patient,
                FullName = patient.FullName,
            };
            await dbContext.AddAsync(user);
            patient.LinkedUser = user;
            await email.SendTempPasswordAsync(request.Email, patient.FullName, password);
            await dbContext.SaveChangesAsync();

            return "تم تسجيل الجساب بنجاح";

        }

        public async Task<string> DeactivePatientProofile(int userId, GetPatientRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Doctor);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.Id == request.PatientId
                                     && p.DoctorId == userId
                                     && p.LinkedUserId != null);

            if (patient == null)
                throw new ForbiddenException("Auth forbidden");

            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == patient.LinkedUserId
                                     && u.IsActive);

            if (user == null)
                throw new BadRequestException("Patient not found");

            patient.LinkedUserId = null;
            user.IsActive = false;
            await dbContext.SaveChangesAsync();

            return "تم تعطيل الحساب بنجاح";
        }

        private string GenerateTempPassword()
        {
            const string chars = "ABCDEFGHJKMNPQRSTUVWXYZabcdefghjkmnpqrstuvwxyz23456789";
            var random = new Random();
            return new string(Enumerable.Repeat(chars, 10)
                .Select(s => s[random.Next(s.Length)])
                .ToArray());
        }
    }
}
