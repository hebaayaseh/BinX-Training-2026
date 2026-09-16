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
        }/// <summary>
         /// Exchanges a refresh token for a new token pair.
         /// </summary>
         /// <remarks>
         /// Refresh tokens are single use. When you call this, the token you sent is
         /// revoked and a new one is returned, so always store the new value. Replaying
         /// an old refresh token returns 401.
         /// </remarks>
         /// <param name="request">The current refresh token.</param>
         /// <response code="200">Returns a new access token and a new refresh token.</response>
         /// <response code="401">Token is unknown, already used, revoked, or expired.</response>
        [HttpPost("refresh-token")]
        [ProducesResponseType(typeof(TokenResponseDto), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
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
