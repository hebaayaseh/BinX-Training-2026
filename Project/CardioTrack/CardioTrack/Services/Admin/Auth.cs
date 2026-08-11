using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class Auth : IAuth
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly ITokenService tokenService;
        public Auth(CardioTrackDbContext dbContext , ITokenService tokenService)
        {
            this.dbContext = dbContext;
            this.tokenService = tokenService;
        }
        public async Task<AdminLoginResponseDto> LoginAsync(AdminLoginRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u=>u.Email ==  request.Email);

            if(user == null)
                throw new KeyNotFoundException("The given email was not found.");

            if (user.PasswordHash != request.Password)
                throw new Exception("Inavild password!");


            var tokens = await tokenService.IssueTokensAsync(
                userId: user.Id,
                name: $"{user.firstName}{user.lastName}",
                email: user.email,
                role: user.role.ToString(),
                centerId: CenterId,
                ownerType: TokenOwnerType.TenantUser
            );
            return new StaffLoginResponseDto
            {
                AccessToken = tokens.AccessToken,
                RefreshToken = tokens.RefreshToken,
                role = user.role.ToString()
            };
        }
    }
}
