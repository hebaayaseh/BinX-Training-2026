using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Interfaces.IDoctor;
using FluentValidation;
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
        [HttpPost("add-Medication")]
        public async Task<IActionResult> AddMedication([FromBody] AddMedicationRequestDto request, IValidator<AddMedicationRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await manage.AddMedicationAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("get-patient-Medication")]
        public async Task<IActionResult> GetPatientMedication([FromBody] GetPatientRequestDto request, IValidator<GetPatientRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await manage.GetPatientMedication(userId, request);
            return Ok(result);
        }
    }
}
