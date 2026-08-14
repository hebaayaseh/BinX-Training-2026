using CardioTrack.Data;
using CardioTrack.DTOs.Patient;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using CardioTrack.Interfaces.IPetient;
using CardioTrack.Models;
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

        public async Task<PatientViewVitalSignReponseDto> PatientViewVitalSignReponse(int userId)
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

            var vitalSigns = await dbContext.vitalSigns
                .Where(v => v.PatientId == patient.Id)
                .Select(s => new VitalSignDto
                {
                    BloodPressureDiastolic = s.BloodPressureDiastolic,
                    BloodPressureSystolic = s.BloodPressureSystolic,
                    Temperature = s.Temperature,
                    RecordedAt = s.RecordedAt,
                    RecordedByUserId = s.RecordedByUserId,
                    HeartRate = s.HeartRate,
                    OxygenSaturation = s.OxygenSaturation

                }).OrderBy(r=>r.RecordedAt)
                .ToListAsync();
            return new PatientViewVitalSignReponseDto { VitalSigns = vitalSigns };

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

        public async Task<ViewMedicationResponseDto> ViewMedication(int userId)
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

            var activeMedications = await dbContext.medications
                .Where(m => m.PatientId == patient.Id
                       && m.EndDate <= DateTime.UtcNow)
                .Select(a => new MedicationResponseDto
                {
                    PrescribedByDoctorId = a.PrescribedByDoctorId,
                    Dosage = a.Dosage,
                    DrugName = a.DrugName,
                    EndDate = a.EndDate,
                    StartDate = a.StartDate,
                    Frequency = a.Frequency
                }).OrderBy(e=>e.EndDate)
                .ToListAsync();

            return new ViewMedicationResponseDto
            {
                activeMedications = activeMedications
            };
        }
    }
}
