using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.ActiveDeactiveActor
{
    [ApiController]
    [Route("api/admin")]
    public class ActiveDeactiveController : ControllerBase
    {
        private readonly IActiveDeactive activeDeactive;
        public ActiveDeactiveController(IActiveDeactive activeDeactive)
        {
            this.activeDeactive = activeDeactive;
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPut("active")]
        public async Task<IActionResult> ActiveActor([FromBody]ActiveDeactiveDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await activeDeactive.ActiveActor(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("deactive")]
        public async Task<IActionResult> DeactiveActor([FromBody] ActiveDeactiveDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await activeDeactive.DeactiveActor(userId, request);
            return Ok(result);
        }
    }
}
