using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class GetPatientService : IGetPatient
    {
        private readonly CardioTrackDbContext dbContext;
        public GetPatientService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<GettPatientResponseDto> GettPatientAsync(int userId)
        {
            var admin = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Admin);

            if (admin == null)
                throw new InvalidTokenException("");

            var doctors = await dbContext.users
                .Include(p=>p.LinkedPatient)
                .Where(d => d.Role == UserRole.Doctor)
                .Select(d => new DoctorPatientDto
                {
                    DoctorId = d.Id,
                    DoctorName = d.FullName,
                    Patients = d.PatientsAsDoctor.Select(n=>new PatientDto
                    {
                        PatientId = n.Id,
                        PatientName = n.FullName
                    }).ToList()
                })
                .ToListAsync();
            return new GettPatientResponseDto
            {
                Doctors = doctors
            };

        }
    }
}
