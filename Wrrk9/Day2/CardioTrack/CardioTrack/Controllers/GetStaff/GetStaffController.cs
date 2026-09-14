using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.GetStaff
{
    [ApiController]
    [Route("api/admin")]
    public class GetStaffController : ControllerBase
    {
        private readonly IGetStaff getStaff;
        public GetStaffController(IGetStaff getStaff)
        {
            this.getStaff = getStaff;
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-staff")]
        public async Task<IActionResult> GetStaff()
        {
            int adminId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await getStaff.GetStaffAsync(adminId);
            return Ok(result);
        }
    }
}
