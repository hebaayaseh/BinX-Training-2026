using CardioTrack.Data;
using CardioTrack.Enums;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.VitalSignsAlert
{
    public class VitalSignAlertEvaluator
    {
        private readonly CardioTrackDbContext dbContext;
        public VitalSignAlertEvaluator(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task TemperatureEvaluate(int vitalSignId , double Temperature)
        {
            var vitalSign = await dbContext.vitalSigns
                .FirstOrDefaultAsync(v => v.Id == vitalSignId);

            if (vitalSign == null)
                throw new BadHttpRequestException("Vital sign not found");

            var alert = new Models.VitalSignAlert
            {
                VitalSignId = vitalSignId,
                PatientId = vitalSign.PatientId,
                AlterType = AlterType.Temperature,
                CreatedAt = DateTime.UtcNow,
                Severity = CheckTemperature(Temperature),
                Message = "The temperature is abnormal"
            };

            
        }
        private Severity CheckTemperature( double Temperature)
        {
            if (Temperature < 35) return Severity.High;
            if (Temperature < 36.1) return Severity.Medium;
            if (Temperature <= 38.5) return Severity.Medium;
            return Severity.High;
        }
    }
}
