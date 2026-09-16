using CardioTrack.Data;
using CardioTrack.DTOs.Token;
using CardioTrack.Enums;
using CardioTrack.ExceptionService;
using CardioTrack.Helper;
using CardioTrack.Interfaces.RefreshToken;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;

namespace CardioTrack.Infrastructure.Services.TokenService
{
    public class TokenService : ITokenService
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly JwtTokenGenerator jwtGenerator;

        public TokenService(CardioTrackDbContext dbContext, JwtTokenGenerator jwtGenerator)
        {
            this.dbContext = dbContext;
            this.jwtGenerator = jwtGenerator;
        }

        public async Task<TokenResponseDto> IssueTokensAsync(int userId, string name, string email, UserRole role)
        {
            var accessToken = jwtGenerator.GenerateToken(userId, name, email, role);
            var refreshTokenValue = GenerateSecureToken();
            var hashedToken = ComputeSha256Hash(refreshTokenValue);

            dbContext.refreshTokens.Add(new RefreshToken
            {
                Token = hashedToken,
                UserRole = role,
                UserId = userId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
            await dbContext.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }

        public async Task<TokenResponseDto> RefreshAsync(string refreshToken)
        {
            var hashedInput = ComputeSha256Hash(refreshToken);

            var existing = await dbContext.refreshTokens
                .FirstOrDefaultAsync(t => t.Token == hashedInput);

            if (existing == null || existing.IsRevoked || existing.ExpiresAt < DateTime.UtcNow)
                throw new InvalidTokenException("Invalid or expired refresh token");

            var user = await dbContext.users.FindAsync(existing.UserId);
            if (user == null)
                throw new InvalidTokenException("Invalid or expired refresh token");

            existing.IsRevoked = true;

            var newRefreshTokenValue = GenerateSecureToken();
            var newHashedToken = ComputeSha256Hash(newRefreshTokenValue);

            dbContext.refreshTokens.Add(new RefreshToken
            {
                Token = newHashedToken,
                UserRole = existing.UserRole,
                UserId = existing.UserId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });
            await dbContext.SaveChangesAsync();

            var newAccessToken = jwtGenerator.GenerateToken(existing.UserId, user.FullName, user.Email, existing.UserRole);

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue
            };
        }

        public async Task LogoutAsync(string refreshToken)
        {
            var hashedInput = ComputeSha256Hash(refreshToken);

            var existing = await dbContext.refreshTokens
                .FirstOrDefaultAsync(t => t.Token == hashedInput);

            if (existing != null)
            {
                existing.IsRevoked = true;
                await dbContext.SaveChangesAsync();
            }
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);
        }

        private string ComputeSha256Hash(string rawText)
        {
            using var sha256 = SHA256.Create();
            var bytes = sha256.ComputeHash(Encoding.UTF8.GetBytes(rawText));
            return Convert.ToBase64String(bytes);
        }
    }
}