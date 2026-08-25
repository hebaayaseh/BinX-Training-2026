using CardioTrack.DTOs.Token;
using CardioTrack.Infrastructure.Services.TokenService;
using CardioTrack.Interfaces.RefreshToken;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Token
{
    [ApiController]
    [Route("api/token")]
    public class TokenController : ControllerBase
    {
        private readonly ITokenService token;
        public TokenController(ITokenService token)
        {
            this.token = token;
        }
        [HttpPost("refresh-token")]
        public async Task<IActionResult> RefreshToken([FromBody] RefreshRequestDto request)
        {
            var result = await token.RefreshAsync(request.RefreshToken);
            return Ok(result);
        }
        [HttpPost("logout")]
        public async Task<IActionResult> Logout([FromBody] LogoutRequestDto request)
        {
            await token.LogoutAsync(request.RefreshToken);
            return Ok(new { message = "تم تسجيل الخروج بنجاح" });
        }
    }
}
