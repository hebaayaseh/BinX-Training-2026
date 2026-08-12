using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Doctor
{
    public class ManageMedicationService : IManageMedication
    {
        private readonly CardioTrackDbContext dbContext;
        public ManageMedicationService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<ManageMedicationResponseDto> ManageMedicationAsync(int userId, ManageMedicationRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u=>u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Doctor);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .Include(u=>u.Doctor)
                .FirstOrDefaultAsync(p => p.Id == request.PatientId
                                     && p.DoctorId == doctor.Id);

            if(patient == null)
                throw new BadRequestException("Patient not found");

            await dbContext.medications
                .AddAsync(new Medication
                {
                    PatientId = request.PatientId,
                    DrugName = request.DrugName,
                    Frequency = request.Frequency,
                    Dosage = request.Dosage,
                    StartDate = request.StartDate,
                    EndDate = request.EndDate,
                    PrescribedByDoctorId = doctor.Id
                
                });

            await dbContext.SaveChangesAsync();
            return new ManageMedicationResponseDto 
            {
                PatientId = request.PatientId,
                DrugName = request.DrugName,
                Frequency = request.Frequency,
                Dosage = request.Dosage,
                StartDate = request.StartDate,
                EndDate = request.EndDate,
                PatientName =patient.FullName
            };


        }
    }
}
