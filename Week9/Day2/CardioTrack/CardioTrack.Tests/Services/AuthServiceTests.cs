using CardioTrack.Data;
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
    public class AuthServiceTests
    {
        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        [Fact]
        public async Task LoginAsync_ValidCredentials_ReturnsTokens()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var user = new User
            {
                FullName = "Dr. Test",
                Email = "doctor@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Test1234@"),
                IsActive = true,
                Role = UserRole.Doctor
            };
            await dbContext.users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var tokenServiceMock = new Mock<ITokenService>();
            tokenServiceMock
                .Setup(t => t.IssueTokensAsync(user.Id, user.FullName, user.Email, user.Role))
                .ReturnsAsync(new TokenResponseDto { AccessToken = "fake-access", RefreshToken = "fake-refresh" });

            var service = new AuthService(dbContext, tokenServiceMock.Object);
            var request = new LoginRequestDto { Email = "doctor@test.com", Password = "Test1234@" };

            // Act
            var result = await service.LoginAsync(request);

            // Assert
            Assert.NotNull(result);
            Assert.Equal("fake-access", result!.AccessToken);
            tokenServiceMock.Verify(
                t => t.IssueTokensAsync(user.Id, user.FullName, user.Email, user.Role),
                Times.Once);
        }

        [Fact]
        public async Task LoginAsync_WrongPassword_ThrowsForbidden()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var user = new User
            {
                FullName = "Dr. Test",
                Email = "doctor@test.com",
                PasswordHash = BCrypt.Net.BCrypt.HashPassword("Correct1234@"),
                IsActive = true,
                Role = UserRole.Doctor
            };
            await dbContext.users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var service = new AuthService(dbContext, Mock.Of<ITokenService>());
            var request = new LoginRequestDto { Email = "doctor@test.com", Password = "WrongPassword" };

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() => service.LoginAsync(request));
        }

        [Fact]
        public async Task LoginAsync_EmailNotFound_ThrowsForbidden()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var service = new AuthService(dbContext, Mock.Of<ITokenService>());
            var request = new LoginRequestDto { Email = "nonexistent@test.com", Password = "Anything123@" };

            // Act & Assert
            await Assert.ThrowsAsync<ForbiddenException>(() => service.LoginAsync(request));
        }
    }
}