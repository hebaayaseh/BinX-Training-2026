using CardioTrack.DTOs.Patient;
using CardioTrack.Interfaces.IPetient;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Patient
{
    [ApiController]
    [Route("api/patient")]
    public class PatientController : ControllerBase
    {
        private readonly IPatient patient;
        public PatientController(IPatient patient)
        {
            this.patient = patient;
        }

        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-appointment")]
        public async Task<IActionResult> ViewAppointment([FromBody] ViewAppointmentRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.ViewAppointment(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-medical-history")]
        public async Task<IActionResult> ViewMedicalHestory()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.ViewMedicalHistory(userId);
            return Ok(result);
        }

        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-vital-signs")]
        public async Task<IActionResult> ViewVitalSigns()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.PatientViewVitalSignReponse(userId);
            return Ok(result);
        }

        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-active-medications")]
        public async Task<IActionResult> ViewActiveMedications()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.ViewMedication(userId);
            return Ok(result);
        }


    }
}
