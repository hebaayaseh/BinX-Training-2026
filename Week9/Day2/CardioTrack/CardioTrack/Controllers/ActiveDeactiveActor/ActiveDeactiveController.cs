using CardioTrack.DTOs.Admin;
using CardioTrack.DTOs.LogIn;
using CardioTrack.Interfaces.IAdmin;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> ActiveActor([FromBody]ActiveDeactiveDto request, IValidator<ActiveDeactiveDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await activeDeactive.ActiveActor(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("deactive")]
        public async Task<IActionResult> DeactiveActor([FromBody] ActiveDeactiveDto request, IValidator<ActiveDeactiveDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await activeDeactive.DeactiveActor(userId, request);
            return Ok(result);
        }
    }
}
