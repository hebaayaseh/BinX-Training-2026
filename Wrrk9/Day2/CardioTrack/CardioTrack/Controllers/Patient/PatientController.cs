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
            var result = await patient.ViewAppointmentAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-medical-history")]
        public async Task<IActionResult> ViewMedicalHestory()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.ViewMedicalHistoryAsync(userId);
            return Ok(result);
        }
        /// <summary>
        /// Returns the vital sign history for the authenticated patient's own record.
        /// </summary>
        /// <remarks>
        /// Read-only. The patient can only ever see their own data — the linked 
        /// Patient record is resolved from the authenticated user's ID, never from 
        /// a client-supplied ID, preventing access to other patients' records.
        /// </remarks>
        /// <response code="200">Returns the patient's vital sign history.</response>
        /// <response code="403">Caller has no linked patient record.</response>
        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-vital-signs")]
        public async Task<IActionResult> ViewVitalSigns()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.PatientViewVitalSignAsync(userId);
            return Ok(result);
        }

        [Authorize(Policy = "PatientOnly")]
        [HttpPost("view-active-medications")]
        public async Task<IActionResult> ViewActiveMedications()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.ViewMedicationAsync(userId);
            return Ok(result);
        }


    }
}
