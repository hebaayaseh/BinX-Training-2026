using CardioTrack.Data;
using CardioTrack.DTOs.EditProfile;
using CardioTrack.ExceptionService;
using CardioTrack.Interfaces.IEmail;
using CardioTrack.Interfaces.IProfile;
using CardioTrack.Models;
using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Asn1.Ocsp;

namespace CardioTrack.Services.Profile
{
    public class EditProfileService : IProfile
    {
        private readonly CardioTrackDbContext dbContext;
        private readonly IEmail email;
        public EditProfileService(CardioTrackDbContext dbContext,IEmail email)
        {
            this.dbContext = dbContext;
            this.email = email;
        }

        public async Task<string> ConfirmEmailCode(int userId, CodeVerify codeVerify)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            var validCode = await dbContext.emailVerificationCodes
                .Where(c => c.UserId == userId
                       && c.Purpose == "change-email"
                       && !c.IsUsed
                       && c.Code == codeVerify.Code
                       && c.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (validCode == null || string.IsNullOrEmpty(validCode.PendingValue))
                throw new ConflictException("Verfiy code");

            user.Email = validCode.PendingValue;
            validCode.IsUsed = true;
            await dbContext.SaveChangesAsync();

            return "تم تعديل الايميل بنجاح";
        }

        public async Task<string> ConfirmPasswordCode(int userId, CodeVerify codeVerify)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            var validCode = await dbContext.emailVerificationCodes
                .Where(c => c.UserId == userId
                       && c.Purpose == "change-password"
                       && !c.IsUsed
                       && c.Code == codeVerify.Code
                       && c.ExpiresAt > DateTime.UtcNow)
                .OrderByDescending(c => c.CreatedAt)
                .FirstOrDefaultAsync();

            if (validCode == null || string.IsNullOrEmpty(validCode.PendingValue))
                throw new ConflictException("Verfiy code");

            user.Email = validCode.PendingValue;
            validCode.IsUsed = true;
            await dbContext.SaveChangesAsync();

            return "تم تعديل كلمة المرور بنجاح";
        }

        public async Task<string> EditEmailRequest(int userId, EditEmailRequestDto request)
        {
            var user = await dbContext.users
                .FirstOrDefaultAsync(u => u.Id == userId
                                     && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            var code = new Random().Next(100000, 999999).ToString();

            await dbContext.emailVerificationCodes
                .AddAsync(new EmailVerificationCode
                {
                    UserId = userId,
                    Code = code,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    Purpose = "change-email",
                    PendingValue = request.Email,

                });

            await dbContext.SaveChangesAsync();
            await email.SendOtpAsync(request.Email, code,"change-email");
            return "تم ارسال الكود الى الايميل";
        }

        public async Task<string> EditPasswordRequest(int userId, EditPasswordRequestDto request)
        {
            var user = await dbContext.users
               .FirstOrDefaultAsync(u => u.Id == userId
                                    && u.IsActive);

            if (user == null)
                throw new InvalidTokenException("Auth unauthorized");

            var code = new Random().Next(100000, 999999).ToString();

            await dbContext.emailVerificationCodes
                .AddAsync(new EmailVerificationCode
                {
                    UserId = userId,
                    Code = code,
                    ExpiresAt = DateTime.UtcNow.AddMinutes(10),
                    Purpose = "change-password",
                    PendingValue = BCrypt.Net.BCrypt.HashPassword(request.Password),

                });

            await dbContext.SaveChangesAsync();
            await email.SendOtpAsync(request.Password, code, "change-password");
            return "تم ارسال الكود الى الايميل";
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
