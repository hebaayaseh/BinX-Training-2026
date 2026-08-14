using CardioTrack.Data;
using CardioTrack.DTOs.Patient;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Interfaces.IPetient;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Patient
{
    public class PatientService : IPatient
    {
        private readonly CardioTrackDbContext dbContext;
        public PatientService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        public async Task<ViewAppointmentResponseDto> ViewAppointment(int userId, ViewAppointmentRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(p => p.Id == userId
                                     && p.IsActive
                                     && p.Role == UserRole.Patient);

            if (user == null)
                throw new ForbiddenException("Auth Forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.LinkedUserId == userId);

            if(patient == null)
                throw new ForbiddenException("Auth Forbidden");

            var Appointments = await dbContext.appointments
                .Where(u => u.PatientId == patient.Id
                       && u.Status == request.AppointmentStatus)
                .Select(a => new AppointmentsDto
                {
                    DoctorId = a.DoctorId,
                    DoctorFullName = a.Doctor.FullName,
                    AppointmantDate = a.AppointmentDate
                    
                }).ToListAsync();
            
            return new ViewAppointmentResponseDto { Appointments = Appointments };
        }

        public async Task<ViewMedicalHistoryResponseDto> ViewMedicalHistory(int userId)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(p => p.Id == userId
                                     && p.IsActive
                                     && p.Role == UserRole.Patient);

            if (user == null)
                throw new ForbiddenException("Auth Forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(p => p.LinkedUserId == userId);

            if (patient == null)
                throw new ForbiddenException("Auth Forbidden");

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
    }
}
