using CardioTrack.DTOs.LabResult;
using CardioTrack.Interfaces.ILabResult;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.LabResult
{
    [ApiController]
    [Route("api/Technician")]
    public class LabResultController : ControllerBase
    {
        private readonly IManageLabResult labResultService;
        public LabResultController(IManageLabResult labResultService)
        {
            this.labResultService = labResultService;
        }

        [Authorize(Policy = "TechnicianOnly")]
        [HttpPost]
        public async Task<IActionResult> CreateLabResult([FromForm] CreateLabResultDto request, IValidator<CreateLabResultDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await labResultService.CreateLabResultAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "LabViewer")]
        [HttpGet]
        public async Task<IActionResult> GetLabResults([FromQuery] GetLabResultDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await labResultService.GetLabResultsAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "TechnicianOnly")]
        [HttpPut("status")]
        public async Task<IActionResult> UpdateLabResultStatus([FromBody] UpdateLabResultStatusRequestDto request, IValidator<UpdateLabResultStatusRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await labResultService.UpdateLabResultStatusAsync(userId, request);
            return Ok(result);
        }
    }
}