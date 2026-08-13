using CardioTrack.DTOs.VitalSign;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.VitalSigns
{
    [ApiController]
    [Route("api/DoctorOrNurse")]
    public class VitalSignsController : ControllerBase
    {
        private readonly IVitalSign vitalSign;
        public VitalSignsController(IVitalSign vitalSign)
        {
            this.vitalSign = vitalSign;
        }

        [Authorize("DoctorOrNurse")]
        [HttpPost("view-vitalsign")]
        public async Task<IActionResult> ViewVitalSigns([FromBody] ViewVitalSignRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.ViewVitalSign(userId, request);    
            return Ok(result);
        }
    }
}
