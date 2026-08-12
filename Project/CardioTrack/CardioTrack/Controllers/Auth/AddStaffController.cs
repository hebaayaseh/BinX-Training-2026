using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Auth
{
    [ApiController]
    [Route("add/staff")]
    public class AddStaffController :ControllerBase
    {
        private readonly IAddStaff addStaff;
        public AddStaffController(IAddStaff addStaff)
        {
            this.addStaff = addStaff;
        }
        [Authorize(Policy = "Admin")]
        [HttpPost("add-doctor")]
        public async Task<IActionResult> AddDoctor([FromBody]AddDoctorRequestDto request)
        {
            var result = await addStaff.AddDoctorAsync(request);
            return Ok(result);
        }
    }
}
