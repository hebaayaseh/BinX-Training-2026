using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IVitalSign;
using CardioTrack.Models;
using CardioTrack.VitalSignsAlert;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.VitalSigns
{
    public class ViewVitalSignService : IVitalSign
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly VitalSignAlertEvaluator signAlertEvaluator;
        public ViewVitalSignService(CardioTrackDbContext dbContext , VitalSignAlertEvaluator signAlertEvaluator)
        {
            this.dbContext = dbContext;
            this.signAlertEvaluator = signAlertEvaluator;
        }

        public async Task<VitalSignDto> AddVitalSign(int userId, AddVitalSignRequestDto request)
        {
            var user = await dbContext.users
                 .FirstOrDefaultAsync(u => u.Id == userId
                                      && u.IsActive
                                      && (u.Role == UserRole.Doctor
                                      || u.Role == UserRole.Nurse));

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            if(user.Role==UserRole.Doctor)
            {
                var DoctorPatient = await dbContext.patients
                    .FirstOrDefaultAsync(p => p.DoctorId == userId
                                        && p.Id == request.PatientId);
                if(DoctorPatient == null)
                    throw new ForbiddenException("Auth forbidden");
            }

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(u => u.Id == request.PatientId);
            if (patient == null)
                throw new BadRequestException("Patient not found");


            var vitalsign = new VitalSign
            {
                PatientId = request.PatientId,
                BloodPressureDiastolic = request.BloodPressureDiastolic,
                BloodPressureSystolic = request.BloodPressureSystolic,
                Temperature = request.Temperature,
                OxygenSaturation = request.OxygenSaturation,
                HeartRate = request.HeartRate,
                RecordedAt = DateTime.UtcNow,
                RecordedByUserId = userId,
            };
            await dbContext.AddAsync(vitalsign);

            await signAlertEvaluator.EvaluateAllAsync(vitalsign);
            await dbContext.SaveChangesAsync();                    

            return new VitalSignDto
            {
                PatientFullName = patient.FullName,
                BloodPressureDiastolic = request.BloodPressureDiastolic,
                BloodPressureSystolic = request.BloodPressureSystolic,
                OxygenSaturation = request.OxygenSaturation,
                HeartRate = request.HeartRate,
                Temperature = request.Temperature,
                RecordedAt = vitalsign.RecordedAt,
                RecordedByUseName = user.FullName,
                RecordedByUserId = user.Id
            };


        }

        public async Task<DoctorViewVitalSignAlertResponceDto> DoctorViewVitalSignAlert(int userId)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var alerts = await dbContext.vitalSignAlerts
                .Include(p => p.Patient)
                .Where(a => a.Patient.DoctorId == userId)
                .Select(r=>new DoctorAlertDto
                {
                    PatientId = r.PatientId,
                    PatientName=r.Patient.FullName,
                    Severity = r.Severity,
                    CreatedAt = r.CreatedAt,
                    AlterType=r.AlterType
                }).ToListAsync();
            return new DoctorViewVitalSignAlertResponceDto
            {
                alerts = alerts
            };
        }

        public async Task<NurseViewVitalSignAlertResponceDto> NurseViewVitalSignAlert(int userId)
        {
            var doctor = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && u.Role == UserRole.Nurse);

            if (doctor == null)
                throw new ForbiddenException("Auth forbidden");

            var alerts = await dbContext.vitalSignAlerts
                .Include(p=>p.Patient)
                .ThenInclude(d=>d.Doctor)
                .Select(r => new NurseAlertDto
                {
                    DoctorId = r.Patient.DoctorId,
                    DoctorName = r.Patient.Doctor.FullName,
                    PatientId = r.PatientId,
                    PatientName = r.Patient.FullName,
                    Severity = r.Severity,
                    CreatedAt = r.CreatedAt,
                    AlterType = r.AlterType
                }).ToListAsync();

            return new NurseViewVitalSignAlertResponceDto
            {
                alerts = alerts
            };
        }

        public async Task<ViewVitalSignResponseDto> ViewVitalSign(int userId, ViewVitalSignRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive
                                     && (u.Role == UserRole.Doctor
                                     || u.Role == UserRole.Nurse));

            if (user == null)
                throw new ForbiddenException("Auth forbidden");

            var patient = await dbContext.patients
                .FirstOrDefaultAsync(u => u.Id == request.PatientId);
            if (patient == null)
                throw new BadRequestException("Patient not found");

            var vitalSign = await dbContext.vitalSigns
                .Where(s => s.PatientId == request.PatientId)
                .Select(p=>new VitalSignDto
                {
                    VitalSignId = p.Id,
                    PatientFullName = patient.FullName,
                    BloodPressureDiastolic = p.BloodPressureDiastolic,
                    OxygenSaturation =p.OxygenSaturation,
                    RecordedAt = p.RecordedAt,
                    HeartRate = p.HeartRate,
                    RecordedByUserId = p.RecordedByUserId,
                    RecordedByUseName = user.FullName,
                    BloodPressureSystolic = p.BloodPressureSystolic,
                    Temperature = p.Temperature
                })
                .OrderBy(d => d.RecordedAt)
                .ToListAsync();

            return new ViewVitalSignResponseDto
            {
                VitalSigns = vitalSign
            };
            
        }
    }
}
