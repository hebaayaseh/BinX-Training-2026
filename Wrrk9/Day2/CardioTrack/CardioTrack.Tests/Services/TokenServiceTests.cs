using CardioTrack.Data;
using CardioTrack.Enums;
using CardioTrack.Helper;
using CardioTrack.Infrastructure.Services.TokenService;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Xunit;

namespace CardioTrack.Tests.Services
{
    public class TokenServiceTests
    {
        private static CardioTrackDbContext CreateDbContext()
        {
            var options = new DbContextOptionsBuilder<CardioTrackDbContext>()
                .UseInMemoryDatabase(Guid.NewGuid().ToString())
                .Options;
            return new CardioTrackDbContext(options);
        }

        private static JwtTokenGenerator CreateJwtGenerator()
        {
            var configValues = new Dictionary<string, string?>
            {
                { "Jwt:Key", "ThisIsATestKeyThatIsLongEnough123456" },
                { "Jwt:Issuer", "CardioTrackTest" },
                { "Jwt:Audience", "CardioTrackTestUsers" }
            };
            var config = new ConfigurationBuilder().AddInMemoryCollection(configValues).Build();
            return new JwtTokenGenerator(config);
        }

        [Fact]
        public async Task RefreshAsync_ExpiredToken_ThrowsException()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var user = new User { FullName = "Test", Email = "t@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Doctor };
            await dbContext.users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var service = new TokenService(dbContext, CreateJwtGenerator());
            var issued = await service.IssueTokensAsync(user.Id, user.FullName, user.Email, user.Role);

            // Manually expire the token
            var storedToken = await dbContext.refreshTokens.FirstAsync();
            storedToken.ExpiresAt = DateTime.UtcNow.AddDays(-1);
            await dbContext.SaveChangesAsync();

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.RefreshAsync(issued.RefreshToken));
        }

        [Fact]
        public async Task RefreshAsync_RevokedToken_ThrowsException()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var user = new User { FullName = "Test", Email = "t2@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Doctor };
            await dbContext.users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var service = new TokenService(dbContext, CreateJwtGenerator());
            var issued = await service.IssueTokensAsync(user.Id, user.FullName, user.Email, user.Role);
            await service.LogoutAsync(issued.RefreshToken);   // revokes it

            // Act & Assert
            await Assert.ThrowsAsync<Exception>(() => service.RefreshAsync(issued.RefreshToken));
        }

        [Fact]
        public async Task RefreshAsync_ValidToken_ReturnsNewTokensAndRevokesOld()
        {
            // Arrange
            await using var dbContext = CreateDbContext();
            var user = new User { FullName = "Test", Email = "t3@test.com", PasswordHash = "hash", IsActive = true, Role = UserRole.Doctor };
            await dbContext.users.AddAsync(user);
            await dbContext.SaveChangesAsync();

            var service = new TokenService(dbContext, CreateJwtGenerator());
            var issued = await service.IssueTokensAsync(user.Id, user.FullName, user.Email, user.Role);

            // Act
            var refreshed = await service.RefreshAsync(issued.RefreshToken);

            // Assert
            Assert.NotEqual(issued.RefreshToken, refreshed.RefreshToken);   // Rotation happened
            Assert.NotEqual(issued.AccessToken, refreshed.AccessToken);

            var oldTokenRecord = await dbContext.refreshTokens
                .Where(t => t.UserId == user.Id)
                .OrderBy(t => t.CreatedAt)
                .FirstAsync();
            Assert.True(oldTokenRecord.IsRevoked);   // Old token marked revoked
        }
    }
}