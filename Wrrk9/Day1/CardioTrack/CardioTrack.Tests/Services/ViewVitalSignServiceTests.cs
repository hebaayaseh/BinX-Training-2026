using CardioTrack.Data;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Models;
using CardioTrack.Services.Patient;
using CardioTrack.Services.VitalSigns;
using CardioTrack.VitalSignsAlert;
using Microsoft.Data.Sqlite;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Tests.Services
{
    public class ViewVitalSignServiceTests
    {
        private SqliteConnection CreateConnection()
        {
            var connection = new SqliteConnection("DataSource=:memory:");
            connection.Open();

            return connection;
        }

        private CardioTrackDbContext CreateDbContext(SqliteConnection connection)
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseSqlite(connection)
                .Options;

            var dbContext = new CardioTrackDbContext(options);

            dbContext.Database.EnsureCreated();

            return dbContext;
        }

        [Fact]
        public async Task AddVitalSign_ValidDoctorAndPatient_ReturnsVitalSignDto()
        {
            // Arrange
            await using var connection = CreateConnection();
            await using var dbContext = CreateDbContext(connection);

            var doctor = new User
            {
                Id = 1,
                FullName = "Doctor Test",
                Email = "doctor@test.com",
                PasswordHash = "TestPasswordHash",
                PhoneNumber = "0790000000",
                IsActive = true,
                Role = UserRole.Doctor
            };

            var patient = new Patient
            {
                Id = 1,
                FullName = "Patient Test",
                PhoneNumber = "0790000000",
                Address = "Test Address",
                DoctorId = 1
            };

            dbContext.users.Add(doctor);
            dbContext.patients.Add(patient);

            await dbContext.SaveChangesAsync();

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var service = new ViewVitalSignService(
                dbContext,
                evaluator,
                new FakeAuditLog());

            var request = new AddVitalSignRequestDto
            {
                PatientId = 1,
                BloodPressureDiastolic = 80,
                BloodPressureSystolic = 120,
                Temperature = 37m,
                OxygenSaturation = 98,
                HeartRate = 80
            };

            // Act
            var result = await service.AddVitalSign(1, request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("Patient Test", result.PatientFullName);
            Assert.Equal(120, result.BloodPressureSystolic);
            Assert.Equal(80, result.BloodPressureDiastolic);
            Assert.Equal(37m, result.Temperature);
            Assert.Equal(98, result.OxygenSaturation);
            Assert.Equal(80, result.HeartRate);
            Assert.Equal("Doctor Test", result.RecordedByUseName);
            Assert.Equal(1, result.RecordedByUserId);
        }

        [Fact]
        public async Task AddVitalSign_InvalidUser_ThrowsForbiddenException()
        {
            // Arrange
            await using var connection = CreateConnection();
            await using var dbContext = CreateDbContext(connection);

            var evaluator = new VitalSignAlertEvaluator(dbContext);

            var service = new ViewVitalSignService(
                dbContext,
                evaluator,
                new FakeAuditLog());

            var request = new AddVitalSignRequestDto
            {
                PatientId = 1,
                BloodPressureDiastolic = 80,
                BloodPressureSystolic = 120,
                Temperature = 37m,
                OxygenSaturation = 98,
                HeartRate = 80
            };

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(
                () => service.AddVitalSign(999, request));
        }
        [Fact]
        public async Task PatientViewVitalSignReponse_UnlinkedUser_ThrowsForbidden()
        {
            await using var connection = CreateConnection();
            await using var dbContext = CreateDbContext(connection);
            var user = new User { FullName = "Ghost", Email = "ghost@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Patient };
            await dbContext.users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var service = new PatientService(dbContext);

            await Assert.ThrowsAsync<ForbiddenException>(() => service.PatientViewVitalSignReponse(user.Id));
        }

        [Fact]
        public async Task PatientViewVitalSignReponse_ValidLinkedPatient_ReturnsOwnVitalSignsOnly()
        {
            await using var connection = CreateConnection();
            await using var dbContext = CreateDbContext(connection);
            var doctor = new User { FullName = "Dr. X", Email = "dx@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Doctor };
            var patientUser = new User { FullName = "Patient A", Email = "pa@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Patient };
            await dbContext.users.AddRangeAsync(doctor, patientUser);
            await dbContext.SaveChangesAsync();

            var patient = new Patient { FullName = "Patient A", DoctorId = doctor.Id, LinkedUserId = patientUser.Id, DateOfBirth = DateTime.UtcNow.AddYears(-30), Gender = Gender.Female, PhoneNumber = "0590000001", Address = "Addr", BloodType = BloodType.A_Positive };
            var otherPatient = new Patient { FullName = "Patient B", DoctorId = doctor.Id, DateOfBirth = DateTime.UtcNow.AddYears(-40), Gender = Gender.Male, PhoneNumber = "0590000002", Address = "Addr", BloodType = BloodType.O_Negative };
            await dbContext.patients.AddRangeAsync(patient, otherPatient);
            await dbContext.SaveChangesAsync();

            await dbContext.vitalSigns.AddRangeAsync(
                new VitalSign { PatientId = patient.Id, HeartRate = 75, BloodPressureSystolic = 110, BloodPressureDiastolic = 70, OxygenSaturation = 98, Temperature = 37, RecordedAt = DateTime.UtcNow, RecordedByUserId = doctor.Id },
                new VitalSign { PatientId = otherPatient.Id, HeartRate = 80, BloodPressureSystolic = 120, BloodPressureDiastolic = 80, OxygenSaturation = 97, Temperature = 37, RecordedAt = DateTime.UtcNow, RecordedByUserId = doctor.Id }
            );
            await dbContext.SaveChangesAsync();

            var service = new PatientService(dbContext);

            var result = await service.PatientViewVitalSignAsync(patientUser.Id);

            Assert.Single(result.VitalSigns);   
        }
    }
}