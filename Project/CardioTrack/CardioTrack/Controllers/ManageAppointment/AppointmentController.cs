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
        public async Task<IActionResult> AddAppointment([FromBody]AddApointmentRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await appointment.AddAppointmentAsync(userId, request);
            return Ok(result);
        }
    }
}
