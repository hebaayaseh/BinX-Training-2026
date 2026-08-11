using CardioTrack.Data;
using CardioTrack.DTOs.LogIn;
using CardioTrack.Interfaces.IAdmin;
using CardioTrack.Interfaces.RefreshToken;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class AuthService : IAuth
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly ITokenService tokenService;
        public AuthService(CardioTrackDbContext dbContext, ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }
        public async Task<LoginResponseDto?> LoginAsync(LoginRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Email == request.Email);

            if (user == null) return null;

            var passwordValid = BCrypt.Net.BCrypt.Verify(request.Password, user.PasswordHash);
            if (!passwordValid) return null;


            var tokens = await tokenService.IssueTokensAsync(
                userId: user.Id,
                name: user.FullName,
                email: user.Email,
                role: user.Role
            );
            return new LoginResponseDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken
            };
        }
    }
}
