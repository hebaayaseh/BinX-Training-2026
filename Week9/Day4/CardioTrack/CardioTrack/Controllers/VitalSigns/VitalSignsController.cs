using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Interfaces.IVitalSign;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

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
        public async Task<IActionResult> ViewVitalSigns([FromBody] ViewVitalSignRequestDto request , IValidator<GetPatientRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.ViewVitalSign(userId, request);    
            return Ok(result);
        }
        /// <summary>
        /// Records a vital-sign reading for a patient and evaluates it for alerts.
        /// </summary>
        /// <remarks>
        /// After the reading is saved it is passed through <c>VitalSignAlertEvaluator</c>,
        /// which compares temperature, heart rate, oxygen saturation and blood pressure
        /// against clinical thresholds. Any reading outside the normal range creates a
        /// <c>VitalSignAlert</c> with Medium or High severity, visible to the treating
        /// doctor and to nursing staff. You do not create alerts yourself.
        ///
        /// Sample request:
        ///
        ///     POST /api/DoctorOrNurse/add-vitalsign
        ///     {
        ///        "patientId": 12,
        ///        "temperature": 38.9,
        ///        "heartRate": 122,
        ///        "oxygenSaturation": 91,
        ///        "systolic": 158,
        ///        "diastolic": 96
        ///     }
        ///
        /// </remarks>
        /// <param name="request">The reading to record.</param>
        /// <param name="validator">Injected FluentValidation validator.</param>
        /// <response code="200">Reading saved. Alerts, if any, were created automatically.</response>
        /// <response code="400">Validation failed, or the patient does not exist.</response>
        /// <response code="401">Missing or expired access token.</response>
        /// <response code="403">Caller is not a Doctor or Nurse, or is not linked to this patient.</response>
        [Authorize("DoctorOrNurse")]
        [HttpPost("add-vitalsign")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddVitalSigns([FromBody] AddVitalSignRequestDto request, IValidator<AddVitalSignRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

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
        public async Task<IActionResult> DoctorResolveVitalSignsAlert([FromBody]ResoleVitalSignAlertRequestDto request , IValidator<ResoleVitalSignAlertRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.DoctorResoleVitalSign(userId,request);
            return Ok(result);
        }

        [Authorize("NurseOnly")]
        [HttpPut("nurse-resolve-vitalsignalert")]
        public async Task<IActionResult> NurseResolveVitalSignsAlert([FromBody] ResoleVitalSignAlertRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await vitalSign.NurseResoleVitalSign(userId, request);
            return Ok(result);
        }
    }
}
