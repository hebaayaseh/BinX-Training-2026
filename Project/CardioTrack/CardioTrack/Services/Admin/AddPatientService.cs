using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class AddPatientService : IAddPatient
    {
        private readonly CardioTrackDbContext dbContext;
        public AddPatientService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
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

            if (user == null)
                throw new BadRequestException("Doctor not found");

            var patient = new Models.Patient
            {
                FullName = request.FullName,
                PhoneNumber = request.phoneNumber,
                Address = request.Address,
                BloodType = request.BloodType,
                DateOfBirth = request.DatrOfBirth,
                DoctorId = request.DoctorId,
                Gender = request.Gender
            };
            await dbContext.AddAsync(patient);
            await dbContext.SaveChangesAsync();
            return "تم تسجيل المريض بنجاح";
        }
    }
}
