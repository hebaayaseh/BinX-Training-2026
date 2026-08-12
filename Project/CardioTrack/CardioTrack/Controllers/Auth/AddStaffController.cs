using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Auth
{
    [ApiController]
    [Route("api/admin")]
    public class AddStaffController :ControllerBase
    {
        private readonly IAddStaff addStaff;
        public AddStaffController(IAddStaff addStaff)
        {
            this.addStaff = addStaff;
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-doctor")]
        public async Task<IActionResult> AddDoctor([FromBody]AddDoctorRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await addStaff.AddDoctorAsync(userId , request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-nurse")]
        public async Task<IActionResult> AddNurse([FromBody] AddNurseRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await addStaff.AddNurseAsync(userId, request);
            return Ok(result);
        }

    }
}
