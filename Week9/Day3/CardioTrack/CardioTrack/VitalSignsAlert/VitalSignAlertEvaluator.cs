using CardioTrack.Data;
using CardioTrack.Enums;
using CardioTrack.Interfaces.IVitalSign;
using CardioTrack.Models;
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
       public async Task EvaluateAllAsync(VitalSign vitalSign)
        {
            await EvaluateTemperatureAsync(vitalSign);
            await EvaluateHeartRateAsync(vitalSign);
            await EvaluateBloodPressureAsync(vitalSign);
            await EvaluateOxygenSaturationAsync(vitalSign);
        }

        private async Task EvaluateTemperatureAsync(VitalSign vitalsign)
        {
            var result = CheckTemperature(vitalsign.Temperature);
            if (result.Severity == null) return;// Temperature is normal

            await dbContext.vitalSignAlerts
                .AddAsync(new VitalSignAlert
                {
                    VitalSign = vitalsign,
                    PatientId = vitalsign.PatientId,
                    CreatedAt = DateTime.UtcNow,
                    AlterType = AlterType.Temperature,
                    Severity = result.Severity.Value,
                    Message = result.Message
                });

        }

        public (Severity? Severity, string Message) CheckTemperature(decimal temperature)
        {
            if (temperature < 35m) return (Severity.High, "Temperature critically low");
            if (temperature < 36.1m) return (Severity.Medium, "Temperature slightly below normal");
            if (temperature <= 37.5m) return (null, string.Empty);
            return (Severity.High, "High fever detected");
        }

        private async Task EvaluateBloodPressureAsync(VitalSign vitalsign)
        {
            var result = CheckBloodPressureAsync(vitalsign.BloodPressureSystolic);
            if (result.Severity == null) return; // bloodPressureSystolic is normal
            await dbContext.vitalSignAlerts
                .AddAsync(new VitalSignAlert
                {
                    VitalSign = vitalsign,
                    PatientId = vitalsign.PatientId,
                    CreatedAt = DateTime.UtcNow,
                    AlterType = AlterType.BloodPressure,
                    Severity = result.Severity.Value,
                    Message = result.Message
                });
        }

        private (Severity? Severity, string Message) CheckBloodPressureAsync(double bloodPressureSystolic)
        {
            if (bloodPressureSystolic < 90) return (Severity.High, "Blood pressure critically low ");
            if (bloodPressureSystolic < 120) return (null, string.Empty);
            if (bloodPressureSystolic < 140) return (Severity.Medium, "Blood pressure elevated");
            return (Severity.High, "Blood pressure critically high ");
        }

        private async Task EvaluateHeartRateAsync(VitalSign vitalsign)
        {
            var result = CheckHeartRate(vitalsign.HeartRate);
            if (result.Severity == null) return;

            await dbContext.vitalSignAlerts.AddAsync(new Models.VitalSignAlert
            {
                VitalSign = vitalsign,
                PatientId = vitalsign.PatientId,
                AlterType = AlterType.HeartRate,
                Severity = result.Severity.Value,
                Message = result.Message,
                CreatedAt = DateTime.UtcNow,
                IsResolved = false
            });
        }

        private async Task EvaluateOxygenSaturationAsync(VitalSign vitalsign)
        {
            var result = CheckOxygenSaturation(vitalsign.OxygenSaturation);
            if (result.Severity == null) return;

            await dbContext.vitalSignAlerts.AddAsync(new Models.VitalSignAlert
            {
                VitalSign = vitalsign,
                PatientId = vitalsign.PatientId,
                AlterType = AlterType.OxygenSaturation,
                Severity = result.Severity.Value,
                Message = result.Message,
                CreatedAt = DateTime.UtcNow,
                IsResolved = false
            });
        }
        public (Severity? Severity, string Message) CheckOxygenSaturation(double OxygenSaturation)
        {
            if (OxygenSaturation < 90) return (Severity.High, "Oxygen saturation critically low");
            if (OxygenSaturation < 95) return (Severity.Medium, "Oxygen saturation below normal");
            return (null, string.Empty);
        }

        public (Severity? Severity, string Message) CheckHeartRate(double HeartRate)
        {
            if (HeartRate < 60) return (Severity.High, "Heart rate critically low ");
            if (HeartRate <= 100) return (null, string.Empty);
            if (HeartRate <= 120) return (Severity.Medium, "Heart rate above normal ");
            return (Severity.High, "Heart rate critically high ");
        }
    }
}
