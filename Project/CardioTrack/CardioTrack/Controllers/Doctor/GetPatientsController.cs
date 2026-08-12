using CardioTrack.Interfaces.IDoctor;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Doctor
{
    [ApiController]
    [Route("api/Doctor")]
    public class GetPatientsController : ControllerBase
    {
        private readonly IGetPatients getPatients;
        public GetPatientsController(IGetPatients getPatients)
        {
            this.getPatients = getPatients;
        }
        [Authorize(Policy = "DoctorOnly")]
        [HttpGet("get-patients")]
        public async Task<IActionResult> GetPatients()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await getPatients.GetPatientsAsync(userId);
            return Ok(result);
        }
    }
}
