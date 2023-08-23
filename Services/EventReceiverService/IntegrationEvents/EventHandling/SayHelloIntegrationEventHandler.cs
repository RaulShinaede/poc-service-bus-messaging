using EventBus.Abstractions;
using EventReceiverService.IntegrationEvents.Events;
using EventReceiverService.Services;

namespace EventReceiverService.IntegrationEvents.EventHandling {
    public class SayHelloIntegrationEventHandler : IIntegrationEventHandler<SayHelloIntegrationEvent> {
        public Task Handle(SayHelloIntegrationEvent @event) {
            return Task.Run(() => {
                HelloCounter.Increment();
            });
        }
    }
}
