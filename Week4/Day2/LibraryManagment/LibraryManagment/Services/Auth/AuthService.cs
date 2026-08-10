using LibraryManagment.DTO.AuthDto;
using LibraryManagment.Helper;
using LibraryManagment.Interface.Auth;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace LibraryManagment.Services.Auth
{
    public class AuthService : IAuth
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly JwtSettings jwt;
        private readonly SignInManager<IdentityUser> signInManager;
        public AuthService(UserManager<IdentityUser> userManager, JwtSettings jwt, SignInManager<IdentityUser> signInManager)
        {
            this.userManager = userManager;
            this.jwt = jwt;
            this.signInManager = signInManager;
        }

        public async Task<LoginResponseDto> Login(LoginRequestDto request)
        {
            var user = await userManager.FindByEmailAsync(request.Email);
            if (user == null)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid Email or Password" }
                };
            }
            var result = await signInManager.CheckPasswordSignInAsync(user, request.Password, false);
            if (!result.Succeeded)
            {
                return new LoginResponseDto
                {
                    IsSuccess = false,
                    Errors = new List<string> { "Invalid Email or Password" }
                };
            }
            var token = GenerateJwtToken(user);
            return new LoginResponseDto
            {
                IsSuccess = true,
                Token = token
            };
        }

        private string GenerateJwtToken(IdentityUser user)
        {
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub,user.Id),
                new Claim(JwtRegisteredClaimNames.Email,user.Email ?? string.Empty),
                new Claim(JwtRegisteredClaimNames.Jti,Guid.NewGuid().ToString())

            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var token = new JwtSecurityToken(
                issuer: jwt.Issuer,
                audience: jwt.Audience,
                claims: claims,
                expires: DateTime.Now.AddMinutes(jwt.ExpiryMinutes),
                signingCredentials: creds
            );
            return new JwtSecurityTokenHandler().WriteToken(token);
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
