using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.Patient;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Doctor
{
    public class MedicalHistoryService : IMedicalHistory
    {
        private readonly CardioTrackDbContext dbContext;
        public MedicalHistoryService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<AddMedicalHistoryResponseDto> AddMedicalHistoryAsync(int userId, AddHistoryRequestDto request)
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

            await dbContext.medicalHistories
                .AddAsync(new MedicalHistory
                { 
                   PatientId = request.PatientId,
                   RecordedByDoctorId = doctor.Id,
                   Condition = request.Condition,
                   DiagnosisDate = request.DiagnosisDate,
                   Note = request.Note,
                });
            await dbContext.SaveChangesAsync();
            return new AddMedicalHistoryResponseDto
            {
                PatientId = request.PatientId,
                PatientName = patient.FullName,
                DiagnosisDate = request.DiagnosisDate,
                Note = request.Note,
                Condition = request.Condition
            };

        }

        public async Task<ViewMedicalHistoryResponseDto> ViewPatientMedicalHistoryAsync(int userId, GetPatientRequestDto request)
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

            var medicals = await dbContext.medicalHistories
                .Where(m => m.PatientId == patient.Id)
                .Select(h => new MidicalHistoyDto
                {
                    Condition = h.Condition,
                    RecordedByDoctorId = h.RecordedByDoctorId,
                    Id = h.Id,
                    DiagnosisDate = h.DiagnosisDate,
                    Note = h.Note
                }).ToListAsync();

            return new ViewMedicalHistoryResponseDto { Midicals = medicals };

        }
        public async Task<string> UpdateMedicalHistoryAsync(int userId, UpdateMedicalHistoryRequestDto request)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Doctor);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var history = await dbContext.medicalHistories
                .Include(h => h.Patient)
                .FirstOrDefaultAsync(h => h.Id == request.MedicalHistoryId
                                     && h.Patient.DoctorId == doctor.Id);

            if (history == null)
                throw new BadRequestException("Medical history not found");

            if (!string.IsNullOrEmpty(request.Condition))
                history.Condition = request.Condition;

            if (!string.IsNullOrEmpty(request.Note))
                history.Note = request.Note;

            await dbContext.SaveChangesAsync();

            return "تم تعديل البيانات بنجاح";
        }
    }
}
