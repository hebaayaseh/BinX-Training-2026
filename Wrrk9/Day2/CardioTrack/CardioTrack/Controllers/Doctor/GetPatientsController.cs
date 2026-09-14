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
        /// <summary>
        /// Retrieves a paginated, filterable, sortable list of the requesting doctor's patients.
        /// </summary>
        /// <remarks>
        /// Only returns patients assigned to the authenticated doctor. Results are 
        /// cached per doctor for 5 minutes using Redis; the cache is invalidated 
        /// immediately on any patient create or update.
        /// </remarks>
        /// <param name="query">Pagination, filtering, and sorting options.</param>
        /// <response code="200">Returns the paginated patient list.</response>
        /// <response code="401">Caller is not authenticated as a doctor.</response>
        [Authorize(Policy = "DoctorOnly")]
        [HttpGet("get-patients")]
        public async Task<IActionResult> GetPatients([FromQuery] GetPatientsQueryRequestDto query)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await getPatients.GetPatientsAsync(userId, query);
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
