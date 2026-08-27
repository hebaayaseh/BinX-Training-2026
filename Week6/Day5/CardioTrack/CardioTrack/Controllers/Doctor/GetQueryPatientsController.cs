using CardioTrack.DTOs.Doctor;
using CardioTrack.Interfaces.IQuery;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Doctor
{
    [ApiController]
    [Route("api/Doctor")]
    public class GetQueryPatientsController : ControllerBase
    {
        private readonly IQuery query;
        public GetQueryPatientsController(IQuery query)
        {
            this.query = query;
        }
        [Authorize(Policy = "DoctorOnly")]
        [HttpGet("get-query-patients")]
        public async Task<IActionResult> GetPatients([FromQuery] GetPatientsQueryRequestDto request)
        {
            int userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)!.Value);
            var result = await query.GetPatientsAsync(userId, request);
            return Ok(result);
        }
    }
}
