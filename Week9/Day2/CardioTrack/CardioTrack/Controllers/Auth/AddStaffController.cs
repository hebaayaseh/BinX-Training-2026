using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Controllers.Auth
{
    [ApiController]
    [Route("api/admin")]
    public class AddStaffController :ControllerBase
    {
        private readonly IAddStaff addStaff;
        public AddStaffController(IAddStaff addStaff)
        {
            this.addStaff = addStaff;
        }/// <summary>
         /// Creates a Doctor account and emails the new doctor a temporary password.
         /// </summary>
         /// <remarks>
         /// Admin only. The password is generated server side, hashed with BCrypt, and
         /// sent by email — it is never returned in the response and never logged. The
         /// new account is active immediately; use <c>PUT /api/admin/deactive</c> to
         /// disable it later.
         ///
         /// Sample request:
         ///
         ///     POST /api/admin/add-doctor
         ///     {
         ///        "fullName": "Dr. Sara Khalil",
         ///        "email": "sara.khalil@cardiotrack.com",
         ///        "phoneNumber": "0599123456"
         ///     }
         ///
         /// </remarks>
         /// <param name="request">Name, email and phone number of the new doctor.</param>
         /// <param name="validator">Injected FluentValidation validator.</param>
         /// <response code="200">Account created and the temporary password was emailed.</response>
         /// <response code="400">Email is malformed or the phone number is missing.</response>
         /// <response code="401">Missing or expired access token.</response>
         /// <response code="403">Caller is not an Admin, or the email is already registered.</response>
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-doctor")]
        [ProducesResponseType(typeof(string), StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status401Unauthorized)]
        [ProducesResponseType(StatusCodes.Status403Forbidden)]
        public async Task<IActionResult> AddDoctor([FromBody] AddDoctorRequestDto request, IValidator<AddDoctorRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await addStaff.AddDoctorAsync(userId , request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-nurse")]
        public async Task<IActionResult> AddNurse([FromBody] AddNurseRequestDto request, IValidator<AddNurseRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await addStaff.AddNurseAsync(userId, request);
            return Ok(result);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-technician")]
        public async Task<IActionResult> AddTechnician([FromBody] AddTechnicianRequestDto request)
        {
            
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await addStaff.AddTechnicianAsync(userId, request);
            return Ok(result);
        }

    }
}
