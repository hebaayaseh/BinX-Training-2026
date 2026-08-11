using CardioTrack.Infrastructure.Services.TokenService;
using CardioTrack.Interfaces.RefreshToken;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Token
{
    [ApiController]
    [Route("api-token")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService token;
        public TokenController(ITokenService token)
        {
            this.token = token;
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody]string refreshToken)
        {
            var result = await token.RefreshAsync(refreshToken);
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] string refreshToken)
        {
            await token.LogoutAsync(refreshToken);
            return Ok(new { message = "تم تسجيل الخروج بنجاح" });
        }
    }
}
