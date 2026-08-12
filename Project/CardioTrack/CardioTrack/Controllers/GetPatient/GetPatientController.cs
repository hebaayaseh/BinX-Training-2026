using CardioTrack.Interfaces.IAdmin;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.GetPatient
{
    [ApiController]
    [Route("api/admin")]
    public class GetPatientController : ControllerBase
    {
        private readonly IGetPatient getPatient;
        public GetPatientController(IGetPatient getPatient)
        {
            this.getPatient= getPatient;
        }

        [Authorize(Policy = "AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetPatient()
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await getPatient.GettPatientAsync(userId);
            return Ok(result);
        }
    }
}
