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
        /// Records a new vital sign measurement for a patient.
        /// </summary>
        /// <remarks>
        /// Automatically evaluates the recorded values (heart rate, blood pressure, 
        /// oxygen saturation, temperature) against medical thresholds. If any value 
        /// falls outside the normal range, a VitalSignAlert is generated automatically 
        /// as part of the same operation — no separate call is needed.
        /// </remarks>
        /// <param name="request">The measured vital sign values and the patient they belong to.</param>
        /// <response code="200">Vital sign recorded successfully.</response>
        /// <response code="400">Invalid values or patient not found.</response>
        /// <response code="403">Caller is not authorized to record vitals for this patient.</response>
        [Authorize("DoctorOrNurse")]
        [HttpPost("add-vitalsign")]
        public async Task<IActionResult> AddVitalSigns([FromBody] AddVitalSignRequestDto request,IValidator<AddVitalSignRequestDto> validator)
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
