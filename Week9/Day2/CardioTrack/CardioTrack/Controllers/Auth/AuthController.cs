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
        /// <summary>
        /// Authenticates a user and issues JWT access and refresh tokens.
        /// </summary>
        /// <remarks>
        /// Works for all roles (Admin, Doctor, Nurse, Patient). The returned access 
        /// token includes the user's role as a claim, used by all protected endpoints.
        /// </remarks>
        /// <param name="request">Email and password credentials.</param>
        /// <response code="200">Login succeeded, tokens returned.</response>
        /// <response code="400">Missing or invalid request format.</response>
        /// <response code="401">Invalid email or password.</response>
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
