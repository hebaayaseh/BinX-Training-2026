using CardioTrack.DTOs.EditProfile;
using CardioTrack.Interfaces.IProfile;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Profile
{
    [ApiController]
    [Route("api/editprofile")]
    public class EditProfileController :ControllerBase
    {
        private readonly IProfile profile;
        public EditProfileController(IProfile profile)
        {
            this.profile = profile;
        }

        [Authorize(Policy = "AllActors")]
        [HttpPut("edit-profile")]
        public async Task<IActionResult> EditProfile([FromBody] EditProfileRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await profile.EditProfileAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "AllActors")]
        [HttpPost("edit-email")]
        public async Task<IActionResult> EditEmail([FromBody] EditEmailRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await profile.EditEmailRequest(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "AllActors")]
        [HttpPost("confirm-email")]
        public async Task<IActionResult> ConfirmEmail([FromBody] CodeVerify request)
        {
           
            var result = await profile.ConfirmEmailCode( request);
            return Ok(result);
        }

        [Authorize(Policy = "AllActors")]
        [HttpPost("edit-password")]
        public async Task<IActionResult> EditPasswod([FromBody] EditPasswordRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await profile.EditPasswordRequest(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "AllActors")]
        [HttpPost("confirm-password")]
        public async Task<IActionResult> ConfirmPassword([FromBody] CodeVerify request)
        {
            var result = await profile.ConfirmPasswordCode(request);
            return Ok(result);
        }

        [Authorize(Policy = "AllActors")]
        [HttpGet("staff-view-profile")]
        public async Task<IActionResult> StaffViewProfie()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await profile.viewProfile(userId);
            return Ok(result);
        }

    }
}
