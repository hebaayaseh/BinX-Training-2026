using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Doctor
{
    [ApiController]
    [Route("api/Doctor")]
    public class GetPatientsController : ControllerBase
    {
        private readonly IGetPatients getPatients;
        public GetPatientsController(IGetPatients getPatients)
        {
            this.getPatients = getPatients;
        }
        [Authorize(Policy = "DoctorOnly")]
        [HttpGet("get-patients")]
        public async Task<IActionResult> GetPatients()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await getPatients.GetPatientsAsync(userId);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("get-patient-doctor-or-nurse")]
        public async Task<IActionResult> GetPatient([FromBody] GetPatientRequestDto request , IValidator<GetPatientRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await getPatients.GetPatientAsync(userId,request);
            return Ok(result);
        }
    }
}
