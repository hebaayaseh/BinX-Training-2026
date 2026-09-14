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
        /// <summary>
        /// Schedules a new appointment for a patient with a doctor.
        /// </summary>
        /// <remarks>
        /// Rejects the request if the doctor already has a scheduled appointment 
        /// at the exact same date/time. The appointment fee is calculated 
        /// automatically based on the reason for the visit. If a RelatedAlertId is 
        /// provided, the referenced vital sign alert is resolved as part of the 
        /// same database transaction — both succeed or both roll back together.
        /// </remarks>
        /// <response code="200">Appointment created successfully.</response>
        /// <response code="400">Patient not found or invalid related alert.</response>
        /// <response code="409">Doctor already has an appointment at this exact time.</response>
        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("add-appointment")]
        public async Task<IActionResult> AddAppointment([FromBody]AddDoctorScheduleRequestDto request, IValidator<AddDoctorScheduleRequestDto> validator)
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
