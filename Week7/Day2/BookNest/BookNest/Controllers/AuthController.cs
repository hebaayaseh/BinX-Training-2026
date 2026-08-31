using BookNest.Data;
using BookNest.Dtos;
using BookNest.Helper;
using BookNest.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace BookNest.Controllers
{
    [ApiController]
    [Route("api/auth")]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly JwtTokenGenerator jwtGenerator;
        private readonly BookNestDbContext dbContext;

        public AuthController(UserManager<ApplicationUser> userManager, JwtTokenGenerator jwtGenerator,BookNestDbContext dbContext)
        {
            this.userManager = userManager;
            this.jwtGenerator = jwtGenerator;
            this.dbContext = dbContext;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register(RegisterDto dto)
        {
            var existing = await userManager.FindByEmailAsync(dto.Email);
            if (existing != null)
                return BadRequest("Email already registered");

            await using var transaction = await dbContext.Database.BeginTransactionAsync();
            try
            {
                var user = new ApplicationUser
                {
                    UserName = dto.Email,
                    Email = dto.Email
                };

                var result = await userManager.CreateAsync(user, dto.Password);
                if (!result.Succeeded)
                {
                    await transaction.RollbackAsync();
                    return BadRequest(result.Errors.Select(e => e.Description));
                }

                await userManager.AddToRoleAsync(user, "Member");

                var profile = new MemberProfile
                {
                    FullName = dto.FullName,
                    ApplicationUserId = user.Id,
                    JoinedAt = DateTime.UtcNow
                };

                await dbContext.MemberProfiles.AddAsync(profile);
                await dbContext.SaveChangesAsync();

                await transaction.CommitAsync();

                return Ok(new
                {
                    Message = "Registered successfully",
                    UserId = user.Id,
                    MemberProfileId = profile.Id
                });
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDto dto)
        {
            var user = await userManager.FindByEmailAsync(dto.Email);
            if (user == null)
                return Unauthorized("Invalid credentials");

            var passwordValid = await userManager.CheckPasswordAsync(user, dto.Password);
            if (!passwordValid)
                return Unauthorized("Invalid credentials");

            var roles = await userManager.GetRolesAsync(user);

            var profile = await dbContext.MemberProfiles
                .FirstOrDefaultAsync(p => p.ApplicationUserId == user.Id);

            if (profile == null)
                return Unauthorized("Member profile not found for this account");

            var token = jwtGenerator.GenerateToken(user.Id, user.Email!, profile.Id, profile.FullName, roles);

            return Ok(new { AccessToken = token, MemberProfileId = profile.Id, profile.FullName, Roles = roles });
        }
    }
}