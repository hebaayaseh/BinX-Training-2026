using CardioTrack.Interfaces.IAuditLog;
using CardioTrack.Services.AuditLogservice;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.GetAuditLog
{
    [ApiController]
    [Route("api/Admin")]
    public class GetAuditLogController : ControllerBase
    {
        private readonly IAuditLog log;
        public GetAuditLogController(IAuditLog log)
        {
            this.log = log;
        }
        [Authorize("AdminOnly")]
        [HttpGet]
        public async Task<IActionResult> GetAuditLog()
        {
            var result = await log.GetAuditLog();
            return Ok(result);
        }
    }
}
