using CardioTrack.DTOs.LabRequest;
using CardioTrack.Interfaces.ILabRequest;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.LabRequest
{
    [ApiController]
    [Route("api/Doctor")]
    public class LabRequestController : ControllerBase
    {
        private readonly IManageLabRequest labRequestService;
        public LabRequestController(IManageLabRequest labRequestService)
        {
            this.labRequestService = labRequestService;
        }
        /// <summary>
        /// Orders one or more lab tests for a patient.
        /// </summary>
        /// <remarks>
        /// Each entry in <c>testNames</c> becomes a separate lab request with status
        /// <c>Pending</c>, so ordering three tests creates three rows. Technicians then
        /// move each one to Seen or Collected. <c>Completed</c> is never set by hand —
        /// it happens automatically when a technician uploads the result.
        ///
        /// Sample request:
        ///
        ///     POST /api/Doctor
        ///     {
        ///        "patientId": 12,
        ///        "appointmentId": 45,
        ///        "testNames": [ "CBC", "Lipid Panel", "Troponin I" ]
        ///     }
        ///
        /// </remarks>
        /// <param name="request">Patient, optional appointment, and the tests to order.</param>
        /// <param name="validator">Injected FluentValidation validator.</param>
        /// <response code="200">Returns one created lab request per test name.</response>
        /// <response code="400">Patient not found, empty test list, or the appointment does not belong to this patient.</response>
        /// <response code="401">Missing or expired access token.</response>
        /// <response code="403">Caller is not a Doctor.</response>
        [Authorize(Policy = "DoctorOnly")]
        [HttpPost]
        [ProducesResponseType(typeof(List<LabRequestResponseDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> CreateLabRequest([FromBody] CreateLabRequestDto request, IValidator<CreateLabRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await labRequestService.CreateLabRequestAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "LabViewer")]
        [HttpGet]
        public async Task<IActionResult> GetLabRequests([FromQuery] GetLabRequestsRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await labRequestService.GetLabRequestsAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "TechnicianOnly")]
        [HttpPut("status")]
        public async Task<IActionResult> UpdateLabRequestStatus([FromBody] UpdateLabRequestStatusRequestDto request, IValidator<UpdateLabRequestStatusRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await labRequestService.UpdateLabRequestStatusAsync(userId, request);
            return Ok(result);
        }
    }
}