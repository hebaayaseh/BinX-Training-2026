using CardioTrack.DTOs.LogIn;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Interfaces.IAdmin;
using FluentValidation;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Auth
{
    [ApiController]
    [Route("api/login")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request, IValidator<LoginRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var result = await auth.LoginAsync(request);
            return Ok(result);
        }
    }
}
