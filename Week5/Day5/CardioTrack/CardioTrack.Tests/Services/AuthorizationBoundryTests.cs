using CardioTrack.Data;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Models;
using CardioTrack.Services.Doctor;
using Microsoft.EntityFrameworkCore;
using Xunit;

namespace CardioTrack.Tests.Services
{
    public class AuthorizationBoundaryTests
    {
        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        private static async Task<(User owningDoctor, User otherDoctor, Patient patient)> SeedOwnershipDataAsync(
            CardioTrackDbContext dbContext)
        {
            var owningDoctor = new User
            {
                FullName = "Dr. Owner",
                Email = "owner.doctor@test.com",
                PasswordHash = "hash",
                IsActive = true,
                Role = UserRole.Doctor
            };

            var otherDoctor = new User
            {
                FullName = "Dr. Unauthorized",
                Email = "unauthorized.doctor@test.com",
                PasswordHash = "hash",
                IsActive = true,
                Role = UserRole.Doctor
            };

            await dbContext.users.AddRangeAsync(owningDoctor, otherDoctor);
            await dbContext.SaveChangesAsync();

            var patient = new Patient
            {
                FullName = "Protected Patient",
                DateOfBirth = new DateTime(1988, 4, 12),
                Gender = Gender.Female,
                PhoneNumber = "0598888888",
                Address = "Test Address",
                BloodType = BloodType.O_Positive,
                DoctorId = owningDoctor.Id   
            };
            await dbContext.patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();

            return (owningDoctor, otherDoctor, patient);
        }


        [Fact]
        public async Task ManageMedication_OwningDoctor_Succeeds()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (owningDoctor, _, patient) = await SeedOwnershipDataAsync(dbContext);
            var service = new ManageMedicationService(dbContext);

            var request = new DTOs.Doctor.AddMedicationRequestDto
            {
                PatientId = patient.Id,
                DrugName = "TestDrug",
                Dosage = "10mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow
            };

            // Act
            var result = await service.AddMedicationAsync(owningDoctor.Id, request);

            // Assert
            Assert.Equal(patient.FullName, result.PatientName);
        }

        [Fact]
        public async Task ManageMedication_NonOwningDoctor_ThrowsBadRequest()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (_, otherDoctor, patient) = await SeedOwnershipDataAsync(dbContext);
            var service = new ManageMedicationService(dbContext);

            var request = new DTOs.Doctor.AddMedicationRequestDto
            {
                PatientId = patient.Id,
                DrugName = "TestDrug",
                Dosage = "10mg",
                Frequency = "Once daily",
                StartDate = DateTime.UtcNow
            };

            // Act & Assert: 
            await Assert.ThrowsAsync<BadRequestException>(
                () => service.AddMedicationAsync(otherDoctor.Id, request));
        }


        [Fact]
        public async Task AddMedicalHistory_NonOwningDoctor_ThrowsBadRequest()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (_, otherDoctor, patient) = await SeedOwnershipDataAsync(dbContext);
            var service = new MedicalHistoryService(dbContext);

            var request = new DTOs.Doctor.AddHistoryRequestDto
            {
                PatientId = patient.Id,
                Condition = "Hypertension",
                DiagnosisDate = DateTime.UtcNow,
                Note = "Unauthorized attempt"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => service.AddMedicalHistoryAsync(otherDoctor.Id, request));
        }

        [Fact]
        public async Task AddMedicalHistory_OwningDoctor_Succeeds()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (owningDoctor, _, patient) = await SeedOwnershipDataAsync(dbContext);
            var service = new MedicalHistoryService(dbContext);

            var request = new DTOs.Doctor.AddHistoryRequestDto
            {
                PatientId = patient.Id,
                Condition = "Hypertension",
                DiagnosisDate = DateTime.UtcNow,
                Note = "Authorized entry"
            };

            // Act
            var result = await service.AddMedicalHistoryAsync(owningDoctor.Id, request);

            // Assert
            Assert.Equal(patient.FullName, result.PatientName);
            Assert.Equal("Hypertension", result.Condition);
        }
    }
}