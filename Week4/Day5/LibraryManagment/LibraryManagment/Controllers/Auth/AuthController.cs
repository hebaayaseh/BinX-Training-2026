using LibraryManagment.DTO.AuthDto;
using LibraryManagment.Interface.Auth;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.RateLimiting;

namespace LibraryManagment.Controllers.Auth
{
    [ApiController]
    [Route("api-auth")]
    public class AuthController :ControllerBase
    {
        public readonly IAuth auth;

        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }
        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterRequestDto request)
        {
            var response = await auth.Register(request);
            if (response.IsSuccess)
            {
                return Ok(response);
            }
            return BadRequest(response);
        }

        [HttpPost("login")]
        [EnableRateLimiting("Login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await auth.Login(request);
            if(!result.IsSuccess)
            {
                return Unauthorized(result);
            }
            return Ok(new {token = result.Token});
        }
    }
}
