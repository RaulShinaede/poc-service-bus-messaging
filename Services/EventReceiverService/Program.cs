using EventBus;
using EventBus.Abstractions;
using EventBusRabbitMQ;
using EventBusRabbitMQ.Abstractions;
using EventReceiverService.IntegrationEvents.EventHandling;
using EventReceiverService.IntegrationEvents.Events;
using Microsoft.Extensions.Configuration;
using RabbitMQ.Client;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();
builder.Services.AddSingleton<IEventBusSubscriptionsManager, InMemoryEventBusSubscriptionsManager>();
builder.Services.AddTransient<SayHelloIntegrationEventHandler>();
builder.Services.AddSingleton<IRabbitMQPersistentConnection>(sp => {
    var logger = sp.GetRequiredService<ILogger<DefaultRabbitMQPersistentConnection>>();
    var eventBusSection = builder.Configuration.GetSection("EventBus");

    var factory = new ConnectionFactory() {
        HostName = builder.Configuration.GetConnectionString("EventBus"),
        DispatchConsumersAsync = true
    };

    if (!string.IsNullOrEmpty(eventBusSection["UserName"])) {
        factory.UserName = eventBusSection["UserName"];
    }

    if (!string.IsNullOrEmpty(eventBusSection["Password"])) {
        factory.Password = eventBusSection["Password"];
    }

    var retryCount = eventBusSection.GetValue("RetryCount", 5);

    return new DefaultRabbitMQPersistentConnection(factory, logger, retryCount);
});
builder.Services.AddSingleton<IEventBus, EventBusRabbitMQ.EventBusRabbitMQ>(sp => {
    var eventBusSection = builder.Configuration.GetSection("EventBus");
    var subscriptionClientName = eventBusSection["SubscriptionClientName"];
    var rabbitMQPersistentConnection = sp.GetRequiredService<IRabbitMQPersistentConnection>();
    var logger = sp.GetRequiredService<ILogger<EventBusRabbitMQ.EventBusRabbitMQ>>();
    var eventBusSubscriptionsManager = sp.GetRequiredService<IEventBusSubscriptionsManager>();
    var retryCount = eventBusSection.GetValue("RetryCount", 5);

    return new EventBusRabbitMQ.EventBusRabbitMQ(rabbitMQPersistentConnection, logger, sp, eventBusSubscriptionsManager, subscriptionClientName, retryCount);
});

var app = builder.Build();

if (app.Environment.IsDevelopment()) {
    app.UseSwagger();
    app.UseSwaggerUI();
}

var eventBus = app.Services.GetRequiredService<IEventBus>();
eventBus.Subscribe<SayHelloIntegrationEvent, SayHelloIntegrationEventHandler>();

app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
