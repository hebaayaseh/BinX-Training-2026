using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
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
        public async Task<MedicalHistoryResponseDto> AddMedicalHistoryAsync(int userId, MedicalHistoryRequestDto request)
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
            return new MedicalHistoryResponseDto
            {
                PatientId = request.PatientId,
                PatientName = patient.FullName,
                DiagnosisDate = request.DiagnosisDate,
                Note = request.Note,
                Condition = request.Condition
            };

        }
    }
}
