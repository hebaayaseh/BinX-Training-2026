using CardioTrack.Data;
using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.EntityFrameworkCore;

namespace CardioTrack.Services.Admin
{
    public class Auth : IAuth
    {
        private readonly CardioTrackDbContext dbContext;
        public Auth(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }
        public async Task<AdminLoginResponseDto> LoginAsync(AdminLoginRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u=>u.Email ==  request.Email);

            if(user == null)
                throw new KeyNotFoundException("The given email was not found.");


        }
    }
}
