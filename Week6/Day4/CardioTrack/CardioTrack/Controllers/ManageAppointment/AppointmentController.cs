using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.VitalSign;
using CardioTrack.Interfaces.IDoctor;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Controllers.ManageAppointment
{
    [ApiController]
    [Route("api/DoctorAndNurse")]
    public class AppointmentController : ControllerBase
    {
        private readonly IAppointment appointment;
        public AppointmentController(IAppointment appointment)
        {
            this.appointment = appointment;
        }
        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("add-appointment")]
        public async Task<IActionResult> AddAppointment([FromBody]AddAppointmentRequestDto request, IValidator<AddAppointmentRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.AddAppointmentAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("complete-appointment")]
        public async Task<IActionResult> CompleteAppointment([FromBody] CompleteAppointmentRequestDto request, IValidator<CompleteAppointmentRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.CompleteAppointmentAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("cancel-appointment")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentRequestDto request,IValidator<CancelAppointmentRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.CancelAppointmentAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "NurseOnly")]
        [HttpPost("get-appointments-by-status-to-nurse")]
        public async Task<IActionResult> GetAppointments([FromBody] GetAppointmentsRequestDto request,IValidator<GetAppointmentsRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.GetAppointmentToNurseAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOnly")]
        [HttpPost("get-appointments-by-status-to-doctor")]
        public async Task<IActionResult> GetAppointmentsToDoctor([FromBody] GetDoctorAppointmentRequestDto request, IValidator<GetDoctorAppointmentRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.GetAppointmentToDuctorAsync(userId, request);
            return Ok(result);
        }

    }
}
