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
    public class MedicalHistoryController : ControllerBase
    {
        private readonly IMedicalHistory medicalHistory;
        public MedicalHistoryController(IMedicalHistory medicalHistory)
        {
            this.medicalHistory = medicalHistory;
        }
        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("add-medical-history")]
        public async Task<IActionResult> AddMedicalHistory([FromBody]MedicalHistoryRequestDto request, IValidator<MedicalHistoryRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await medicalHistory.AddMedicalHistoryAsync(userId, request);
            return Ok(result);
        }
    }
}
