using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IEmail;
using CardioTrack.Models;
using CardioTrack.Services.Admin;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CardioTrack.Tests.Services
{
    /// <summary>
    /// GAP #4 (P2) — account creation had no tests. This is the privilege-escalation
    /// surface of the system: whoever can reach it can mint a Doctor account.
    ///
    /// IEmail is mocked so no real SMTP call happens, and so we can assert that the
    /// temporary password is actually delivered (a silent email failure would leave
    /// a new staff member permanently locked out).
    /// </summary>
    public class AddStaffServiceTests
    {
        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        private static User CreateUser(string email, UserRole role, bool isActive = true) => new User
        {
            FullName = $"{role} user",
            Email = email,
            PasswordHash = "hash",
            PhoneNumber = "0599000000",
            IsActive = isActive,
            Role = role
        };

        private static AddDoctorRequestDto ValidDoctorRequest() => new AddDoctorRequestDto
        {
            FullName = "Dr. New Hire",
            Email = "new.doctor@test.com",
            PhoneNumber = "0599123456"
        };

        // ---------------- Authorization ----------------

        [Fact]
        public async Task AddDoctor_CallerIsDoctor_ThrowsForbiddenAndCreatesNothing()
        {
            // Arrange
            await using var db = CreateDbContext();
            var caller = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddAsync(caller);
            await db.SaveChangesAsync();

            var emailMock = new Mock<IEmail>();
            var service = new AddStaffService(db, emailMock.Object, new FakeAuditLog());

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.AddDoctorAsync(caller.Id, ValidDoctorRequest()));

            Assert.Equal(1, await db.users.CountAsync());   // no new account
            emailMock.Verify(e => e.SendTempPasswordAsync(
                It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()), Times.Never);
        }

        [Fact]
        public async Task AddDoctor_CallerIsNurse_ThrowsForbidden()
        {
            await using var db = CreateDbContext();
            var caller = CreateUser("nurse@test.com", UserRole.Nurse);
            await db.users.AddAsync(caller);
            await db.SaveChangesAsync();

            var service = new AddStaffService(db, Mock.Of<IEmail>(), new FakeAuditLog());

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.AddDoctorAsync(caller.Id, ValidDoctorRequest()));
        }

        [Fact]
        public async Task AddDoctor_CallerIsDeactivatedAdmin_ThrowsInvalidToken()
        {
            // A revoked admin must not keep their powers until the JWT expires.
            await using var db = CreateDbContext();
            var caller = CreateUser("ex.admin@test.com", UserRole.Admin, isActive: false);
            await db.users.AddAsync(caller);
            await db.SaveChangesAsync();

            var service = new AddStaffService(db, Mock.Of<IEmail>(), new FakeAuditLog());

            await Assert.ThrowsAsync<InvalidTokenException>(() =>
                service.AddDoctorAsync(caller.Id, ValidDoctorRequest()));
        }

        [Fact]
        public async Task AddDoctor_CallerDoesNotExist_ThrowsInvalidToken()
        {
            await using var db = CreateDbContext();
            var service = new AddStaffService(db, Mock.Of<IEmail>(), new FakeAuditLog());

            await Assert.ThrowsAsync<InvalidTokenException>(() =>
                service.AddDoctorAsync(9999, ValidDoctorRequest()));
        }

        // ---------------- Duplicate email ----------------

        [Fact]
        public async Task AddDoctor_EmailAlreadyUsed_ThrowsForbidden()
        {
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            var existing = CreateUser("new.doctor@test.com", UserRole.Nurse);
            await db.users.AddRangeAsync(admin, existing);
            await db.SaveChangesAsync();

            var service = new AddStaffService(db, Mock.Of<IEmail>(), new FakeAuditLog());

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.AddDoctorAsync(admin.Id, ValidDoctorRequest()));

            Assert.Equal(2, await db.users.CountAsync());
        }

        // ---------------- Happy path ----------------

        [Fact]
        public async Task AddDoctor_ValidAdmin_CreatesActiveDoctorAndSendsTempPassword()
        {
            // Arrange
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            await db.users.AddAsync(admin);
            await db.SaveChangesAsync();

            var emailMock = new Mock<IEmail>();
            var service = new AddStaffService(db, emailMock.Object, new FakeAuditLog());
            var request = ValidDoctorRequest();

            // Act
            var result = await service.AddDoctorAsync(admin.Id, request);

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result));

            var created = await db.users.FirstAsync(u => u.Email == request.Email);
            Assert.Equal(UserRole.Doctor, created.Role);
            Assert.True(created.IsActive);
            Assert.Equal(request.FullName, created.FullName);
            Assert.Equal(request.PhoneNumber, created.PhoneNumber);

            // the temp password must be hashed, never stored in the clear
            Assert.NotEqual(request.Email, created.PasswordHash);
            Assert.StartsWith("$2", created.PasswordHash);

            emailMock.Verify(e => e.SendTempPasswordAsync(
                request.Email,
                request.FullName,
                It.Is<string>(p => p.Length >= 8)), Times.Once);
        }

        [Fact]
        public async Task AddDoctor_TwoCalls_GenerateDifferentTempPasswords()
        {
            // A predictable temp password would let anyone guess a new hire's login.
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            await db.users.AddAsync(admin);
            await db.SaveChangesAsync();

            var captured = new List<string>();
            var emailMock = new Mock<IEmail>();
            emailMock
                .Setup(e => e.SendTempPasswordAsync(It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()))
                .Callback<string, string, string>((_, _, password) => captured.Add(password))
                .Returns(Task.CompletedTask);

            var service = new AddStaffService(db, emailMock.Object, new FakeAuditLog());

            await service.AddDoctorAsync(admin.Id, new AddDoctorRequestDto
            {
                FullName = "Dr. One",
                Email = "one@test.com",
                PhoneNumber = "0599000001"
            });
            await service.AddDoctorAsync(admin.Id, new AddDoctorRequestDto
            {
                FullName = "Dr. Two",
                Email = "two@test.com",
                PhoneNumber = "0599000002"
            });

            Assert.Equal(2, captured.Count);
            Assert.NotEqual(captured[0], captured[1]);
        }

        // ---------------- Nurse / Technician follow the same rules ----------------

        [Fact]
        public async Task AddNurse_CallerIsDoctor_ThrowsForbidden()
        {
            await using var db = CreateDbContext();
            var caller = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddAsync(caller);
            await db.SaveChangesAsync();

            var service = new AddStaffService(db, Mock.Of<IEmail>(), new FakeAuditLog());

            await Assert.ThrowsAsync<ForbiddenException>(() =>
                service.AddNurseAsync(caller.Id, new AddNurseRequestDto
                {
                    FullName = "Nurse X",
                    Email = "nurse.x@test.com",
                    PhoneNumber = "0599000003"
                }));
        }

        [Fact]
        public async Task AddTechnician_ValidAdmin_CreatesTechnicianRole()
        {
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            await db.users.AddAsync(admin);
            await db.SaveChangesAsync();

            var service = new AddStaffService(db, Mock.Of<IEmail>(), new FakeAuditLog());

            await service.AddTechnicianAsync(admin.Id, new AddTechnicianRequestDto
            {
                FullName = "Tech X",
                Email = "tech.x@test.com",
                PhoneNumber = "0599000004"
            });

            var created = await db.users.FirstAsync(u => u.Email == "tech.x@test.com");
            Assert.Equal(UserRole.Technician, created.Role);
            Assert.True(created.IsActive);
        }
    }
}