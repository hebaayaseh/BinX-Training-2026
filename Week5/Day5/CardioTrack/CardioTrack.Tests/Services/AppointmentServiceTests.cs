using CardioTrack.Data;
using CardioTrack.DTOs.Doctor;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Models;
using CardioTrack.Services.Doctor;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Tests.Services
{
    public class AppointmentServiceTests
    {
        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        private static async Task<(User doctor, User nurse, Patient patient)> SeedBasicDataAsync(CardioTrackDbContext dbContext)
        {
            var doctor = new User
            {
                FullName = "Dr. Test",
                Email = "doctor@test.com",
                PasswordHash = "hash",
                IsActive = true,
                Role = UserRole.Doctor,
                PhoneNumber = "2535635"
            };
            var nurse = new User
            {
                FullName = "Nurse Test",
                Email = "nurse@test.com",
                PasswordHash = "hash",
                IsActive = true,
                Role = UserRole.Nurse,
                PhoneNumber = "253563545"
            };
            await dbContext.users.AddRangeAsync(doctor, nurse);
            await dbContext.SaveChangesAsync();

            var patient = new Patient
            {
                FullName = "Test Patient",
                DateOfBirth = new DateTime(1990, 1, 1),
                Gender = Gender.Male,
                PhoneNumber = "0590000000",
                Address = "Test Address",
                BloodType = BloodType.A_Positive,
                DoctorId = doctor.Id,
            };
            await dbContext.patients.AddAsync(patient);
            await dbContext.SaveChangesAsync();

            return (doctor, nurse, patient);
        }

        [Fact]
        public async Task AddAppointment_ValidRequest_CreatesAppointment()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, patient) = await SeedBasicDataAsync(dbContext);
            var service = new AppointmentService(dbContext);

            var request = new AddAppointmentRequestDto
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = DateTime.UtcNow.AddDays(3),
                Reason = "Routine check-up"
            };

            // Act
            var result = await service.AddAppointmentAsync(nurse.Id, request);

            // Assert
            Assert.Equal(patient.FullName, result.PatientName);
            Assert.Single(dbContext.appointments.Local);
            Assert.Equal(AppointmentStatus.Scheduled, dbContext.appointments.Local.Single().Status);
        }

        [Fact]
        public async Task AddAppointment_UserNotDoctorOrNurse_ThrowsForbidden()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, _, patient) = await SeedBasicDataAsync(dbContext);

            var admin = new User
            {
                FullName = "Admin Test",
                Email = "admin@test.com",
                PasswordHash = "hash",
                IsActive = true,
                Role = UserRole.Admin,
                PhoneNumber = "25356563"
            };
            await dbContext.users.AddAsync(admin);
            await dbContext.SaveChangesAsync();

            var service = new AppointmentService(dbContext);
            var request = new AddAppointmentRequestDto
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = DateTime.UtcNow.AddDays(1),
                Reason = "Check-up"
            };

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(
                () => service.AddAppointmentAsync(admin.Id, request));
        }

        [Fact]
        public async Task AddAppointment_PatientNotAssignedToDoctor_ThrowsBadRequest()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, patient) = await SeedBasicDataAsync(dbContext);

            var otherDoctor = new User
            {
                FullName = "Dr. Other",
                Email = "other@test.com",
                PasswordHash = "hash",
                IsActive = true,
                Role = UserRole.Doctor,
                PhoneNumber = "2544635"
            };
            await dbContext.users.AddAsync(otherDoctor);
            await dbContext.SaveChangesAsync();

            var service = new AppointmentService(dbContext);
            var request = new AddAppointmentRequestDto
            {
                PatientId = patient.Id,
                DoctorId = otherDoctor.Id,   
                AppointmentDate = DateTime.UtcNow.AddDays(1),
                Reason = "Check-up"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => service.AddAppointmentAsync(nurse.Id, request));
        }

        [Fact]
        public async Task AddAppointment_DoctorAlreadyBookedAtSameTime_ThrowsBadRequest()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, patient) = await SeedBasicDataAsync(dbContext);
            var service = new AppointmentService(dbContext);

            var appointmentDate = DateTime.UtcNow.AddDays(2);

            await dbContext.appointments.AddAsync(new Appointment
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = appointmentDate,
                Reason = "Existing appointment",
                Status = AppointmentStatus.Scheduled,
                CreatedByUserId = nurse.Id
            });
            await dbContext.SaveChangesAsync();

            var request = new AddAppointmentRequestDto
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = appointmentDate,   
                Reason = "Duplicate slot attempt"
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => service.AddAppointmentAsync(nurse.Id, request));
        }


        [Fact]
        public async Task CancelAppointment_ValidRequest_SetsStatusToCanceled()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, patient) = await SeedBasicDataAsync(dbContext);

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = DateTime.UtcNow.AddDays(1),
                Reason = "To be canceled",
                Status = AppointmentStatus.Scheduled,
                CreatedByUserId = nurse.Id
            };
            await dbContext.appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();

            var service = new AppointmentService(dbContext);
            var request = new CancelAppointmentRequestDto
            {
                AppointmentId = appointment.Id,
                DoctorId = doctor.Id
            };

            // Act
            await service.CancelAppointmentAsync(nurse.Id, request);

            // Assert
            Assert.Equal(AppointmentStatus.Canceled, appointment.Status);
        }

        [Fact]
        public async Task CancelAppointment_AlreadyCompletedAppointment_ThrowsBadRequest()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, patient) = await SeedBasicDataAsync(dbContext);

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = DateTime.UtcNow.AddDays(-1),
                Reason = "Already completed",
                Status = AppointmentStatus.Completed,   
                CreatedByUserId = nurse.Id
            };
            await dbContext.appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();

            var service = new AppointmentService(dbContext);
            var request = new CancelAppointmentRequestDto
            {
                AppointmentId = appointment.Id,
                DoctorId = doctor.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => service.CancelAppointmentAsync(nurse.Id, request));
        }


        [Fact]
        public async Task CompleteAppointment_ValidRequest_SetsStatusToCompleted()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, patient) = await SeedBasicDataAsync(dbContext);

            var appointment = new Appointment
            {
                PatientId = patient.Id,
                DoctorId = doctor.Id,
                AppointmentDate = DateTime.UtcNow.AddHours(-1),
                Reason = "To complete",
                Status = AppointmentStatus.Scheduled,
                CreatedByUserId = nurse.Id
            };
            await dbContext.appointments.AddAsync(appointment);
            await dbContext.SaveChangesAsync();

            var service = new AppointmentService(dbContext);
            var request = new CompleteAppointmentRequestDto
            {
                AppointmentId = appointment.Id,
                DoctorId = doctor.Id
            };

            // Act
            await service.CompleteAppointmentAsync(nurse.Id, request);

            // Assert
            Assert.Equal(AppointmentStatus.Completed, appointment.Status);
        }

        [Fact]
        public async Task CompleteAppointment_NotFound_ThrowsBadRequest()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var (doctor, nurse, _) = await SeedBasicDataAsync(dbContext);
            var service = new AppointmentService(dbContext);

            var request = new CompleteAppointmentRequestDto
            {
                AppointmentId = 999,   
                DoctorId = doctor.Id
            };

            // Act & Assert
            await Assert.ThrowsAsync<BadRequestException>(
                () => service.CompleteAppointmentAsync(nurse.Id, request));
        }
    }
}
