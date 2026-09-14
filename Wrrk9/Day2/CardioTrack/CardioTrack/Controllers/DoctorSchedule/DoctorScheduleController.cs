using CardioTrack.DTOs.Doctor;
using CardioTrack.DTOs.DoctorSchedule;
using CardioTrack.Interfaces.IDoctorSchedule;
using FluentValidation;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace CardioTrack.Controllers.DoctorSchedule
{
    [ApiController]
    [Route("api/AdminOnly")]
    public class DoctorScheduleController : ControllerBase
    {
        private readonly IDoctorSchedule schedule;
        public DoctorScheduleController(IDoctorSchedule schedule)
        {
            this.schedule = schedule;
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-doctor-schedule/{userId}")]
        public async Task<IActionResult> AddDoctorSchedule(int userId , [FromBody] AddDoctorSheduleRequestDto request, IValidator<AddDoctorScheduleRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var result = await schedule.AddDoctorSheduleAsync(userId, request);
            return Ok(result);
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("update-doctor-schedule")]
        public async Task<IActionResult> UpdateDoctorSchedule([FromBody] UpdateDoctorScheduleRequestDto request, IValidator<AddDoctorScheduleRequestDto> validator)
        {
            var validationResult = await validator.ValidateAsync(request);
            if (!validationResult.IsValid)
                return BadRequest(validationResult.Errors.Select(e => e.ErrorMessage));

            var result = await schedule.UpdaeDoctorScheduleAsync( request);
            return Ok(result);
        }

    }
}
