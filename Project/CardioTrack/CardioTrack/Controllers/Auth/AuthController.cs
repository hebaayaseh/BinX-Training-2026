using CardioTrack.DTOs.LogIn;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Auth
{
    [ApiController]
    [Route("api-login")]
    public class AuthController : ControllerBase
    {
        private readonly IAuth auth;
        public AuthController(IAuth auth)
        {
            this.auth = auth;
        }
        [HttpPost]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto request)
        {
            var result = await auth.LoginAsync(request);
            if (result == null)
                throw new UnauthorizedAccessException();

            return Ok(result);
        }
    }
}
