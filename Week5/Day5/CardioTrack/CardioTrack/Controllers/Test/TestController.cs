using Microsoft.AspNetCore.Mvc;

namespace CardioTrack.Controllers.Diagnostics
{
    [ApiController]
    [Route("api/diagnostics")]
    public class DiagnosticsController : ControllerBase
    {
        [HttpGet("trigger-error")]
        public IActionResult TriggerUnhandledException()
        {
            throw new InvalidOperationException("This is a deliberate test exception with sensitive internal details that should NEVER reach the client.");
        }

        [HttpGet("trigger-app-exception")]
        public IActionResult TriggerAppException()
        {
            throw new CardioTrack.ExceptionService.BadRequestException("This is a deliberate BadRequestException for testing.");
        }
    }
}