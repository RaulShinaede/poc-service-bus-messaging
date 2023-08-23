using EventReceiverService.Services;
using Microsoft.AspNetCore.Mvc;

namespace EventReceiverService.Controllers {
    [ApiController]
    [Route("/counter")]
    public class HelloCounterController : ControllerBase {
        [HttpGet]
        [Route("count")]
        public IActionResult GetCount() {
            return Ok($"You say hello {HelloCounter.GetCount()} times!");
        }
    }
}
