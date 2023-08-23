using EventBus.Abstractions;
using EventBus.Events;

namespace EventEmitterService.IntegrationEvents {
    public class IntegrationEventService {

        private readonly IEventBus _eventBus;
        public IntegrationEventService(IEventBus eventBus) {
            _eventBus = eventBus;
        }

        public async Task PublishThroughEventBusAsync(IntegrationEvent evt) {
            _eventBus.Publish(evt);
        }
    }
}
