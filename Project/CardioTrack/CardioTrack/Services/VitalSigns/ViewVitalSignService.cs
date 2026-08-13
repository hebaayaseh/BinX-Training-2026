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

        public async Task<ViewVitalSignResponceDto> ViewVitalSign(int userId, ViewVitalSignRequestDto request)
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

            return new ViewVitalSignResponceDto
            {
                VitalSigns = vitalSign
            };
            
        }
    }
}
