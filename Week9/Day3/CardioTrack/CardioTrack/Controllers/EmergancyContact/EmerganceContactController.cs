using CardioTrack.DTOs.EmerganceContact;
using CardioTrack.Interfaces.IEmergancyContact;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.EmergancyContact
{
    [ApiController]
    [Route("api/Admin")]
    public class EmerganceContactController : ControllerBase
    {
        private readonly IEmergancyContact emergancy;
        public EmerganceContactController(IEmergancyContact emergancy)
        {
            this.emergancy = emergancy;
        }
        [Authorize(Policy = "AdminOnly")]
        [HttpPost("add-patient-emergancy-contacts")]
        public async Task<IActionResult> AddEmergancyContacts([FromBody] AddEmergancyContactRequestDto request)
        {
            var result = await emergancy.AddEmergancyContactAsync(request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet("get-patient-emergancy-contacts/{patientId}")]
        public async Task<IActionResult> GetEmergancyContacts(int patientId)
        {
            var result = await emergancy.GetEmergancyContactsAsync(patientId);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpDelete("delete-patient-emergancy-contacts")]
        public async Task<IActionResult> DeleteEmergancyContacts([FromBody] RemoveEmergencyContactRequestDto request)
        {
            var result = await emergancy.RemoveEmergancyContactAsync(request);
            return Ok(result);
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpPut("update-patient-emergancy-contacts")]
        public async Task<IActionResult> UpdateEmergancyContacts([FromBody] UpdateEmerganceContactRequestDto request)
        {
            var result = await emergancy.UpdateEmerganceContactAsync(request);
            return Ok(result);
        }

    }
}
