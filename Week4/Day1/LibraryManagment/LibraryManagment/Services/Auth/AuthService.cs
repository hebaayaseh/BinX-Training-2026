using LibraryManagment.DTO.AuthDto;
using LibraryManagment.Interface.Auth;
using Microsoft.AspNetCore.Identity;

namespace LibraryManagment.Services.Auth
{
    public class AuthService : IAuth
    {
        private readonly UserManager<IdentityUser> userManager;
        public AuthService(UserManager<IdentityUser> userManager)
        {
            this.userManager = userManager;
        }   

        public async Task<RegisterResponceDto> Register(RegisterRequestDto request)
        {
            var user = new IdentityUser
            {
                UserName = request.Email,
                Email = request.Email
            };
            var result = await userManager.CreateAsync(user, request.Password);
            return new RegisterResponceDto
            {
                IsSuccess = result.Succeeded,
                Message = result.Succeeded ? "User created successfully" : string.Join(", ", result.Errors.Select(e => e.Description))
            };
        }
    }
}
