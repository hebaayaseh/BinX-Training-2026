using CardioTrack.DTOs.VitalSign;
using CardioTrack.Interfaces.IVitalSign;
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

        [Authorize("DoctorOrNurse")]
        [HttpPost("add-vitalsign")]
        public async Task<IActionResult> AddVitalSigns([FromBody] AddVitalSignRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.AddVitalSign(userId, request);
            return Ok(result);
        }

        [Authorize("DoctorOnly")]
        [HttpGet("doctor-view-vitalsignalert")]
        public async Task<IActionResult> DoctorViewVitalSignsAlert()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.DoctorViewVitalSignAlert(userId);
            return Ok(result);
        }

        [Authorize("NurseOnly")]
        [HttpGet("nurse-view-vitalsignalert")]
        public async Task<IActionResult> NurseViewVitalSignsAlert()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.NurseViewVitalSignAlert(userId);
            return Ok(result);
        }

        [Authorize("DoctorOnly")]
        [HttpPut("doctor-resolve-vitalsignalert")]
        public async Task<IActionResult> DoctorResolveVitalSignsAlert([FromBody]ResoleVitalSignAlertRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.DoctorResoleVitalSign(userId,request);
            return Ok(result);
        }
    }
}
