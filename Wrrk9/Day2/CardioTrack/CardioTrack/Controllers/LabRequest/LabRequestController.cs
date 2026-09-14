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

        [Authorize(Policy = "DoctorOnly")]
        [HttpPost]
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