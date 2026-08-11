using CardioTrack.Data;
using CardioTrack.DTOs.Token;
using CardioTrack.Helper;
using CardioTrack.Interfaces.RefreshToken;
using Microsoft.EntityFrameworkCore;
using Sehatak.Application.DTOs.Auth;
using Sehatak.Application.DTOs.Exceptions;
using Sehatak.Application.Interfaces.IAuth;
using Sehatak.Domain.Entities.SharedEntities;
using Sehatak.Domain.Enums.SharedEnums;
using Sehatak.Infrastructure.Data;
using Sehatak.Infrastructure.Security;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace Sehatak.Infrastructure.Services.tokenService
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

        public async Task<TokenResponseDto> IssueTokensAsync(int userId, string name, string email, string role, int? centerId, TokenOwnerType ownerType)
        {
            var accessToken = jwtGenerator.GenerateToken(userId, name, email, role);
            var refreshTokenValue = GenerateSecureToken();

            dbContext.RefreshTokens.Add(new RefreshToken
            {
                Token = refreshTokenValue,
                OwnerType = ownerType,
                UserId = userId,
                CenterId = centerId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await dbContext.SaveChangesAsync();

            return new TokenResponseDto
            {
                AccessToken = accessToken,
                RefreshToken = refreshTokenValue
            };
        }

        private string GenerateSecureToken()
        {
            var randomBytes = new byte[64];
            using var rng = RandomNumberGenerator.Create();
            rng.GetBytes(randomBytes);
            return Convert.ToBase64String(randomBytes);

        }

        public async Task LogoutAsync(string refreshToken)
        {
            var existing = await dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (existing != null)
            {
                existing.IsRevoked = true;
                await dbContext.SaveChangesAsync();
            }
        }

        public async Task<TokenResponseDto> RefreshAsync(string refreshToken)
        {
            var existing = await dbContext.RefreshTokens
                .FirstOrDefaultAsync(t => t.Token == refreshToken);

            if (existing == null || existing.IsRevoked || existing.ExpiresAt < DateTime.UtcNow)
                throw new Exception("Auth invalid refresh token");

            string name, email, role;

            if (existing.CenterId == null)
                throw new Exception("Auth invalid refresh token");

            var user = await dbContext.users.FindAsync(existing.UserId);
            if (user == null)
                throw new Exception("Auth invalid refresh token");

            name = $"{user.firstName} {user.lastName}";
            email = user.email!;
            role = user.role.ToString();
            

            existing.IsRevoked = true;

            var newRefreshTokenValue = GenerateSecureToken();
            dbContext.RefreshTokens.Add(new RefreshToken
            {
                Token = newRefreshTokenValue,
                OwnerType = existing.OwnerType,
                UserId = existing.UserId,
                CenterId = existing.CenterId,
                ExpiresAt = DateTime.UtcNow.AddDays(7)
            });

            await dbContext.SaveChangesAsync();

            var newAccessToken = jwtGenerator.GenerateToken(existing.UserId, name, email, role, existing.CenterId);

            return new TokenResponseDto
            {
                AccessToken = newAccessToken,
                RefreshToken = newRefreshTokenValue
            };
        }
    }
}

