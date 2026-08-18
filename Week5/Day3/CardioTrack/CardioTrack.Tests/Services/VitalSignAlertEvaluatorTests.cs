using CardioTrack.Data;
using CardioTrack.Enums;
using CardioTrack.Models;
using CardioTrack.VitalSignsAlert;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Tests.Services
{
    public class VitalSignAlertEvaluatorTests
    {
        private readonly VitalSignAlertEvaluator evaluator;

        public VitalSignAlertEvaluatorTests()
        {
            evaluator = new VitalSignAlertEvaluator(null!);
        }


        [Fact]
        public void CheckTemperature_Normal_ReturnsNoAlert()
        {
            // Arrange
            var temperature = 37m;

            // Act
            var result = evaluator.CheckTemperature(temperature);

            // Assert
            Assert.Null(result.Severity);
            Assert.Equal(string.Empty, result.Message);
        }

        [Fact]
        public void CheckTemperature_BelowCriticalLimit_ReturnsHighSeverity()
        {
            // Arrange
            var temperature = 34.9m;

            // Act
            var result = evaluator.CheckTemperature(temperature);

            // Assert
            Assert.Equal(Severity.High, result.Severity);
            Assert.Equal(
                "Temperature critically low",
                result.Message);
        }

        [Fact]
        public void CheckTemperature_AtCriticalLimit_ReturnsMediumSeverity()
        {
            // Arrange
            var temperature = 35m;

            // Act
            var result = evaluator.CheckTemperature(temperature);

            // Assert
            Assert.Equal(Severity.Medium, result.Severity);
            Assert.Equal(
                "Temperature slightly below normal",
                result.Message);
        }

        [Fact]
        public void CheckTemperature_AtNormalUpperLimit_ReturnsNoAlert()
        {
            // Arrange
            var temperature = 37.5m;

            // Act
            var result = evaluator.CheckTemperature(temperature);

            // Assert
            Assert.Null(result.Severity);
        }

        [Fact]
        public void CheckTemperature_AboveNormalLimit_ReturnsHighSeverity()
        {
            // Arrange
            var temperature = 37.6m;

            // Act
            var result = evaluator.CheckTemperature(temperature);

            // Assert
            Assert.Equal(Severity.High, result.Severity);
            Assert.Equal(
                "High fever detected",
                result.Message);
        }



        [Fact]
        public void CheckHeartRate_Normal_ReturnsNoAlert()
        {
            // Arrange
            var heartRate = 80;

            // Act
            var result = evaluator.CheckHeartRate(heartRate);

            // Assert
            Assert.Null(result.Severity);
            Assert.Equal(string.Empty, result.Message);
        }

        [Fact]
        public void CheckHeartRate_BelowNormalLimit_ReturnsHighSeverity()
        {
            // Arrange
            var heartRate = 59;

            // Act
            var result = evaluator.CheckHeartRate(heartRate);

            // Assert
            Assert.Equal(Severity.High, result.Severity);
            Assert.Equal(
                "Heart rate critically low ",
                result.Message);
        }

        [Fact]
        public void CheckHeartRate_AtLowerNormalLimit_ReturnsNoAlert()
        {
            // Arrange
            var heartRate = 60;

            // Act
            var result = evaluator.CheckHeartRate(heartRate);

            // Assert
            Assert.Null(result.Severity);
        }

        [Fact]
        public void CheckHeartRate_AtUpperNormalLimit_ReturnsNoAlert()
        {
            // Arrange
            var heartRate = 100;

            // Act
            var result = evaluator.CheckHeartRate(heartRate);

            // Assert
            Assert.Null(result.Severity);
        }

        [Fact]
        public void CheckHeartRate_AboveNormalLimit_ReturnsMediumSeverity()
        {
            // Arrange
            var heartRate = 101;

            // Act
            var result = evaluator.CheckHeartRate(heartRate);

            // Assert
            Assert.Equal(Severity.Medium, result.Severity);
            Assert.Equal(
                "Heart rate above normal ",
                result.Message);
        }

        [Fact]
        public void CheckHeartRate_AboveCriticalLimit_ReturnsHighSeverity()
        {
            // Arrange
            var heartRate = 121;

            // Act
            var result = evaluator.CheckHeartRate(heartRate);

            // Assert
            Assert.Equal(Severity.High, result.Severity);
            Assert.Equal(
                "Heart rate critically high ",
                result.Message);
        }


        

        [Fact]
        public void CheckOxygenSaturation_Normal_ReturnsNoAlert()
        {
            // Arrange
            var oxygen = 98;

            // Act
            var result = evaluator.CheckOxygenSaturation(oxygen);

            // Assert
            Assert.Null(result.Severity);
            Assert.Equal(string.Empty, result.Message);
        }

        [Fact]
        public void CheckOxygenSaturation_BelowCriticalLimit_ReturnsHighSeverity()
        {
            // Arrange
            var oxygen = 89;

            // Act
            var result = evaluator.CheckOxygenSaturation(oxygen);

            // Assert
            Assert.Equal(Severity.High, result.Severity);
            Assert.Equal(
                "Oxygen saturation critically low",
                result.Message);
        }

        [Fact]
        public void CheckOxygenSaturation_AtCriticalLimit_ReturnsMediumSeverity()
        {
            // Arrange
            var oxygen = 90;

            // Act
            var result = evaluator.CheckOxygenSaturation(oxygen);

            // Assert
            Assert.Equal(Severity.Medium, result.Severity);
            Assert.Equal(
                "Oxygen saturation below normal",
                result.Message);
        }

        [Fact]
        public void CheckOxygenSaturation_AtNormalUpperLimit_ReturnsNoAlert()
        {
            // Arrange
            var oxygen = 95;

            // Act
            var result = evaluator.CheckOxygenSaturation(oxygen);

            // Assert
            Assert.Null(result.Severity);
        }



        [Fact]
        public async Task EvaluateAllAsync_BloodPressureBelowNormal_CreatesHighAlert()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = CreateNormalVitalSign();
            vitalSign.BloodPressureSystolic = 89;

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            var alert = dbContext.vitalSignAlerts
                .Local
                .Single();

            Assert.Equal(
                AlterType.BloodPressure,
                alert.AlterType);

            Assert.Equal(
                Severity.High,
                alert.Severity);

            Assert.Equal(
                "Blood pressure critically low ",
                alert.Message);
        }

        [Fact]
        public async Task EvaluateAllAsync_BloodPressureAtNormalLimit_DoesNotCreateAlert()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = CreateNormalVitalSign();
            vitalSign.BloodPressureSystolic = 110;

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            Assert.Empty(
                dbContext.vitalSignAlerts.Local);
        }

        [Fact]
        public async Task EvaluateAllAsync_BloodPressureElevated_CreatesMediumAlert()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = CreateNormalVitalSign();
            vitalSign.BloodPressureSystolic = 130;

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            var alert = dbContext.vitalSignAlerts
                .Local
                .Single();

            Assert.Equal(
                AlterType.BloodPressure,
                alert.AlterType);

            Assert.Equal(
                Severity.Medium,
                alert.Severity);

            Assert.Equal(
                "Blood pressure elevated",
                alert.Message);
        }

        [Fact]
        public async Task EvaluateAllAsync_BloodPressureCriticallyHigh_CreatesHighAlert()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = CreateNormalVitalSign();
            vitalSign.BloodPressureSystolic = 140;

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            var alert = dbContext.vitalSignAlerts
                .Local
                .Single();

            Assert.Equal(
                AlterType.BloodPressure,
                alert.AlterType);

            Assert.Equal(
                Severity.High,
                alert.Severity);

            Assert.Equal(
                "Blood pressure critically high ",
                alert.Message);
        }



        [Fact]
        public async Task EvaluateAllAsync_WhenHeartRateIsCritical_CreatesAlert()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = CreateNormalVitalSign();
            vitalSign.HeartRate = 130;

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            var alerts = dbContext.vitalSignAlerts.Local.ToList();

            Assert.Single(alerts);

            Assert.Equal(
                Severity.High,
                alerts[0].Severity);

            Assert.Equal(
                AlterType.HeartRate,
                alerts[0].AlterType);

            Assert.Equal(
                "Heart rate critically high ",
                alerts[0].Message);

            Assert.Equal(
                1,
                alerts[0].PatientId);
        }

        [Fact]
        public async Task EvaluateAllAsync_WhenAllVitalSignsAreNormal_DoesNotCreateAlerts()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = CreateNormalVitalSign();

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            Assert.Empty(
                dbContext.vitalSignAlerts.Local);
        }

        [Fact]
        public async Task EvaluateAllAsync_WhenMultipleVitalSignsAreCritical_CreatesMultipleAlerts()
        {
            // Arrange
            await using var dbContext = CreateDbContext();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var vitalSign = new VitalSign
            {
                PatientId = 1,
                HeartRate = 130,
                Temperature = 38m,
                BloodPressureSystolic = 150,
                OxygenSaturation = 88
            };

            // Act
            await evaluator.EvaluateAllAsync(vitalSign);

            // Assert
            var alerts = dbContext.vitalSignAlerts.Local;

            Assert.Equal(4, alerts.Count);

            Assert.Contains(
                alerts,
                a => a.AlterType == AlterType.HeartRate
                     && a.Severity == Severity.High);

            Assert.Contains(
                alerts,
                a => a.AlterType == AlterType.Temperature
                     && a.Severity == Severity.High);

            Assert.Contains(
                alerts,
                a => a.AlterType == AlterType.BloodPressure
                     && a.Severity == Severity.High);

            Assert.Contains(
                alerts,
                a => a.AlterType == AlterType.OxygenSaturation
                     && a.Severity == Severity.High);
        }



        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(
                    Guid.NewGuid().ToString())
                .Options;

            return new CardioTrackDbContext(options);
        }

        private static VitalSign CreateNormalVitalSign()
        {
            return new VitalSign
            {
                PatientId = 1,
                HeartRate = 80,
                Temperature = 37m,
                BloodPressureSystolic = 110,
                OxygenSaturation = 98
            };
        }
    }
}