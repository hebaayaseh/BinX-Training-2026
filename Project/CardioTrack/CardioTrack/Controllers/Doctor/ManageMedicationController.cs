using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Doctor
{
    [ApiController]
    [Route("api/Doctor")]
    public class ManageMedicationController : ControllerBase
    {
        private readonly IManageMedication manage;
        public ManageMedicationController(IManageMedication manage)
        {
            this.manage = manage;
        }

        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("manage-Medication")]
        public async Task<IActionResult> ManageMedication([FromBody] ManageMedicationRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await manage.ManageMedicationAsync(userId, request);
            return Ok(result);
        }
    }
}
