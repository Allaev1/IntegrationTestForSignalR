using BlazorSignalR.Shared.HubMessages;
using Microsoft.AspNetCore.Mvc;
using TestSignalR;

namespace BlazorSignalR.Server.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HubController : ControllerBase
    {
        private readonly ITestHubDispatcher _dispatcher;

        public HubController(ITestHubDispatcher dispatcher)
        {
            _dispatcher = dispatcher;
        }

        [HttpPost("test")]
        public async Task<IActionResult> Test([FromBody] Notification notification)
        {
            await _dispatcher.Dispatch(notification);
            return Ok();
        }
    }
}
