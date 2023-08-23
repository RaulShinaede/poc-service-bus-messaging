using EventEmitterService.IntegrationEvents;
using EventEmitterService.IntegrationEvents.Events;
using Microsoft.AspNetCore.Mvc;

namespace EventEmitterService.Controllers {
    [ApiController]
    [Route("/")]
    public class SayHelloController : ControllerBase {

        private readonly IntegrationEventService _eventService;
        public SayHelloController(IntegrationEventService eventService) {
            _eventService = eventService;
        }

        [HttpPost]
        [Route("sayHello")]
        public async Task<IActionResult> SayHello() {

            await _eventService.PublishThroughEventBusAsync(new SayHelloIntegrationEvent());

            return Ok("Hello!!"); 
        }
    }
}
