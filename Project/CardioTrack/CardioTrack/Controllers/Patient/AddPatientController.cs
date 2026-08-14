using CardioTrack.DTOs.Admin;
using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Patient
{
    [ApiController]
    [Route("api/Admin")]
    public class AddPatientController : ControllerBase
    {
        private readonly IAddPatient patient;
        public AddPatientController(IAddPatient patient)
        {
            this.patient = patient;
        }
        [Authorize("AdminOnly")]
        [HttpPost("add-patient")]
        public async Task<IActionResult> AddPatientAsync([FromBody]AddPatientRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await patient.AddPatientAsync(userId, request);
            return Ok(result);
        }
    }
}
