using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Controllers.Doctor
{
    [ApiController]
    [Route("api/Doctor")]
    public class ActivaOrDeactivePatientController :ControllerBase
    {
        private readonly IActiveDeactivePatient activePatient;
        public ActivaOrDeactivePatientController(IActiveDeactivePatient activePatient)
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

        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("deactive-patient")]
        public async Task<IActionResult> DeactivePatient([FromBody] GetPatientRequestDto request , IValidator<GetPatientRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await activePatient.DeactivePatientProofile(userId, request);
            return Ok(result);
        }
    }
}
