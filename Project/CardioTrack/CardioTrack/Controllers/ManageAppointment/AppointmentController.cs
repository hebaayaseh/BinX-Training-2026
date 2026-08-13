using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IDoctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

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
        public async Task<IActionResult> AddAppointment([FromBody]AddAppointmentRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.AddAppointmentAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("complete-appointment")]
        public async Task<IActionResult> CompleteAppointment([FromBody] CompleteAppointmentRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.CompleteAppointmentAsync(userId, request);
            return Ok(result);
        }

        [Authorize(Policy = "DoctorOrNurse")]
        [HttpPost("cancel-appointment")]
        public async Task<IActionResult> CancelAppointment([FromBody] CancelAppointmentRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.CancelAppointmentAsync(userId, request);
            return Ok(result);
        }

    }
}
