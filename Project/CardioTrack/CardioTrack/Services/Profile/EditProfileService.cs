using CardioTrack.Data;
using CardioTrack.DTOs.EditProfile;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IProfile;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CardioTrack.Services.Profile
{
    public class EditProfileService : IProfile
    {
        private readonly CardioTrackDbContext dbContext;
        public EditProfileService(CardioTrackDbContext dbContext)
        {
            this.dbContext = dbContext;
        }

        

        public async Task<EditProfileResponseDto> EditProfileAsync(int userId, EditProfileRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            if(request.FullName!=null)
                user.FullName=request.FullName;

            if(request.PhoneNumber!=null)
                user.PhoneNumber=request.PhoneNumber;

            await dbContext.SaveChangesAsync();

            return new EditProfileResponseDto 
            { 
                UserId = userId,
                PhoneNumber=user.PhoneNumber,
                FullName = user.FullName,
            };

        }
    }
}
