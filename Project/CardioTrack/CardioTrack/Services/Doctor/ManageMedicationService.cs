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
        public async Task<AddMedicationResponseDto> AddMedicationAsync(int userId, AddMedicationRequestDto request)
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
                    PrescribedByDoctorId = doctor.Id,
                    IsActive = true

                });

            await dbContext.SaveChangesAsync();
            return new AddMedicationResponseDto 
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

        public Task<string> DeactiveMedicationAsync(int userId, DeactiveMedicationRequestDto request)
        {
            throw new NotImplementedException();
        }

        public async Task<GetPatientMedicationResponseDto> GetPatientMedication(int userId, GetPatientRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Doctor);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .Include(u => u.Doctor)
                .FirstOrDefaultAsync(p => p.Id == request.PatientId
                                     && p.DoctorId == doctor.Id);

            if (patient == null)
                throw new BadRequestException("Patient not found");

            var medication = await dbContext.medications
                .FirstOrDefaultAsync(m => m.PatientId == request.PatientId
                                     && m.PrescribedByDoctorId == userId);

            if (medication == null)
                throw new BadRequestException("Medication not found");

            return new GetPatientMedicationResponseDto 
            {
                MedicationId = medication.Id,
                DrugName = medication.DrugName,
                Frequency = medication.Frequency,
                Dosage = medication.Dosage,
                StartDate = medication.StartDate,
                EndDate = medication.EndDate,
                PatientName = patient.FullName

            };

        }

        public async Task<string> UpdateMedicationAsync(int userId, UpdateMedicationRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Doctor);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");


            var medication = await dbContext.medications
                .FirstOrDefaultAsync(m => m.PrescribedByDoctorId == userId);

            if (medication == null)
                throw new BadRequestException("Medication not found");

            if (request.EndDate != null)
                medication.EndDate = request.EndDate;

            if (request.DrugName != null)
                medication.DrugName = request.DrugName;

            if (request.Frequency != null)
                medication.Frequency = request.Frequency;

            if (request.Dosage != null)
                medication.Dosage = request.Dosage;

            if (request.StartDate != null)
                medication.StartDate = (DateTime)request.StartDate;

            await dbContext.SaveChangesAsync();
            return "تم تعديل البيانات بنجاح";
            
        }
    }
}
