using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Doctor
{
    public class ViewVitalSignService : IVitalSign
    {
        private readonly CardioTrackDbContext dbContext;
        public ViewVitalSignService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
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
                .Include(p => p.Patient)
                .Where(s => s.PatientId == request.PatientId)
                .Select(p=>new VitalSign
                {
                    VitalSignId = p.Id,
                    PatientFullName = patient.FullName,
                    BloodPressureDiastolic = p.BloodPressureDiastolic,
                    OxygenSaturation =p.OxygenSaturation,
                    RecordedAt = p.RecordedAt,
                    HeartRate = p.HeartRate,
                    RecordedByUserId = p.RecordedByUserId,
                    RecordedByUseName = user.FullName,
                    BloodPressureSystolic = p.BloodPressureDiastolic,
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
