using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.EntityFrameworkCore;
using System.Reflection.Metadata.Ecma335;

namespace CardioTrack.Services.Doctor
{
    public class GetPatientsService : IGetPatients
    {
        private readonly CardioTrackDbContext dbContext;
        public GetPatientsService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<GetPatientsDto> GetPatientsAsync(int userId)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.Role == UserRole.Doctor
                                     && u.IsActive);

            if (doctor == null)
                throw new InvalidTokenException("Auth forbidden");

            var patients = await dbContext.patients
                .Where(p => p.DoctorId == doctor.Id)
                .Select(d => new PatientsDto
                {
                    PatientId = d.Id,
                    PhoneNumber = d.PhoneNumber,
                    Gender = d.Gender,
                    BloodType = d.BloodType,
                    Address = d.Address,
                    FullName = d.FullName,
                    DateOfBirth =d.DateOfBirth,
                    
                }).ToListAsync();

            return new GetPatientsDto 
            {
                Patients = patients
            };


        }
    }
}
