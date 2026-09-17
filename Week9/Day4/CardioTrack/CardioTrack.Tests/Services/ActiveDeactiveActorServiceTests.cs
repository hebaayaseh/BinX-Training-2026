using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.DTOs.LogIn;
using CardioTrack.DTOs.Token;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.RefreshToken;
using CardioTrack.Models;
using CardioTrack.Services.Admin;
using Microsoft.EntityFrameworkCore;
using Moq;
using Xunit;

namespace CardioTrack.Tests.Services
{
    /// <summary>
    /// GAP #5 (P2) — enabling and disabling accounts had no tests, even though it is
    /// the only way to revoke someone's access.
    ///
    /// The last test in this file is the important one: it checks that a deactivated
    /// user can no longer log in. See the note above it — it fails against the
    /// current AuthService and passes once the one-line fix is applied.
    /// </summary>
    public class ActiveDeactiveActorServiceTests
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
            PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test1234@"),
            PhoneNumber = "0599000000",
            IsActive = isActive,
            Role = role
        };

        // ---------------- Deactivate ----------------

        [Fact]
        public async Task DeactiveActor_ValidAdminAndActiveTarget_SetsIsActiveFalse()
        {
            // Arrange
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            var target = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddRangeAsync(admin, target);
            await db.SaveChangesAsync();

            var service = new ActiveDeactiveActorService(db, new FakeAuditLog());

            // Act
            var result = await service.DeactiveActor(admin.Id, new ActiveDeactiveDto { ActorId = target.Id });

            // Assert
            Assert.False(string.IsNullOrWhiteSpace(result));
            var updated = await db.users.FirstAsync(u => u.Id == target.Id);
            Assert.False(updated.IsActive);
        }

        [Fact]
        public async Task DeactiveActor_TargetAlreadyInactive_ThrowsBadRequest()
        {
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            var target = CreateUser("doctor@test.com", UserRole.Doctor, isActive: false);
            await db.users.AddRangeAsync(admin, target);
            await db.SaveChangesAsync();

            var service = new ActiveDeactiveActorService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.DeactiveActor(admin.Id, new ActiveDeactiveDto { ActorId = target.Id }));
        }

        [Fact]
        public async Task DeactiveActor_TargetDoesNotExist_ThrowsBadRequest()
        {
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            await db.users.AddAsync(admin);
            await db.SaveChangesAsync();

            var service = new ActiveDeactiveActorService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.DeactiveActor(admin.Id, new ActiveDeactiveDto { ActorId = 9999 }));
        }

        [Fact]
        public async Task DeactiveActor_CallerIsDeactivated_ThrowsInvalidToken()
        {
            await using var db = CreateDbContext();
            var caller = CreateUser("ex.admin@test.com", UserRole.Admin, isActive: false);
            var target = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddRangeAsync(caller, target);
            await db.SaveChangesAsync();

            var service = new ActiveDeactiveActorService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<InvalidTokenException>(() =>
                service.DeactiveActor(caller.Id, new ActiveDeactiveDto { ActorId = target.Id }));

            var target2 = await db.users.FirstAsync(u => u.Id == target.Id);
            Assert.True(target2.IsActive);
        }

        // ---------------- Activate ----------------

        [Fact]
        public async Task ActiveActor_InactiveTarget_SetsIsActiveTrue()
        {
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            var target = CreateUser("doctor@test.com", UserRole.Doctor, isActive: false);
            await db.users.AddRangeAsync(admin, target);
            await db.SaveChangesAsync();

            var service = new ActiveDeactiveActorService(db, new FakeAuditLog());

            await service.ActiveActor(admin.Id, new ActiveDeactiveDto { ActorId = target.Id });

            var updated = await db.users.FirstAsync(u => u.Id == target.Id);
            Assert.True(updated.IsActive);
        }

        [Fact]
        public async Task ActiveActor_TargetAlreadyActive_ThrowsBadRequest()
        {
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            var target = CreateUser("doctor@test.com", UserRole.Doctor, isActive: true);
            await db.users.AddRangeAsync(admin, target);
            await db.SaveChangesAsync();

            var service = new ActiveDeactiveActorService(db, new FakeAuditLog());

            await Assert.ThrowsAsync<BadRequestException>(() =>
                service.ActiveActor(admin.Id, new ActiveDeactiveDto { ActorId = target.Id }));
        }

        // ---------------- The one that matters ----------------

        /// <summary>
        /// ⚠️ THIS TEST FAILS AGAINST THE CURRENT CODE — ON PURPOSE.
        ///
        /// AuthService.LoginAsync never checks user.IsActive, so a deactivated
        /// account can still log in and receive a valid 30-minute access token.
        /// Deactivation is therefore cosmetic.
        ///
        /// Fix in Services/Admin/AuthService.cs:
        ///     if (user == null || !user.IsActive)
        ///         throw new ForbiddenException("Email not exist!");
        ///
        /// Apply the fix, then this test goes green. If you are not allowed to
        /// change production code in this lab, mark it [Fact(Skip = "...")] and
        /// raise it as a bug instead of deleting it.
        /// </summary>
        [Fact]
        public async Task DeactivatedUser_CannotLogInAnymore()
        {
            // Arrange
            await using var db = CreateDbContext();
            var admin = CreateUser("admin@test.com", UserRole.Admin);
            var doctor = CreateUser("doctor@test.com", UserRole.Doctor);
            await db.users.AddRangeAsync(admin, doctor);
            await db.SaveChangesAsync();

            var deactivateService = new ActiveDeactiveActorService(db, new FakeAuditLog());
            await deactivateService.DeactiveActor(admin.Id, new ActiveDeactiveDto { ActorId = doctor.Id });

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock
                .Setup(t => t.IssueTokensAsync(
                    It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRole>()))
                .ReturnsAsync(new TokenResponseDto { AccessToken = "a", RefreshToken = "r" });

            var authService = new AuthService(db, tokenServiceMock.Object);

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() =>
                authService.LoginAsync(new LoginRequestDto
                {
                    Email = "doctor@test.com",
                    Password = "Test1234@"
                }));

            // and no token should ever have been minted for them
            tokenServiceMock.Verify(t => t.IssueTokensAsync(
                It.IsAny<int>(), It.IsAny<string>(), It.IsAny<string>(), It.IsAny<UserRole>()), Times.Never);
        }
    }
}