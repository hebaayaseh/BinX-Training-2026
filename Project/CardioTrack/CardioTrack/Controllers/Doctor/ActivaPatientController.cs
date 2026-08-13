using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Doctor
{
    [ApiController]
    [Route("api/Doctor")]
    public class ActivaPatientController :ControllerBase
    {
        private readonly IActivePatient activePatient;
        public ActivaPatientController(IActivePatient activePatient)
        {
            this.activePatient = activePatient;
        }
        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("active-patient")]
        public async Task<IActionResult> ActivePatient([FromBody]ActivePatientProfileRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await activePatient.ActivePatientProfile(userId, request);
            return Ok(result);
        }
    }
}
