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
        /// Authenticates a user and issues a token pair.
        /// </summary>
        /// <remarks>
        /// Works for every role. Returns a short-lived access token (30 minutes) and a
        /// refresh token (7 days). Send the access token on every other call as
        /// <c>Authorization: Bearer {token}</c>.
        ///
        /// Sample request:
        ///
        ///     POST /api/login
        ///     {
        ///        "email": "doctor@cardiotrack.com",
        ///        "password": "Doctor@2026"
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Email and password.</param>
        /// <param name="validator">Injected FluentValidation validator.</param>
        /// <response code="200">Login succeeded. Returns the access and refresh tokens.</response>
        /// <response code="400">Email is malformed or the password field is empty.</response>
        /// <response code="403">Email does not exist or the password is wrong.</response>
        [HttpPost]
        [ProducesResponseType(typeof(LoginResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
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
