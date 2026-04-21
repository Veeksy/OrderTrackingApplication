using MassTransit;
using Microsoft.Extensions.Logging;
using OrderTrackingApplication.Contracts.IntegrationEvents;
using OrderTrackingApplication.Domain.Events;
using System.Text.Json;

namespace OrderTrackingApplication.Application.EventsHandlers.Order.OrderUpdate;

public class OrderUpdateEventHandler : IOrderUpdateEventHandler
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<OrderUpdateEventHandler> _logger;

    public OrderUpdateEventHandler(IPublishEndpoint publishEndpoint, ILogger<OrderUpdateEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }
    
    public async Task HandleOrderStatusChangedAsync(OrderStatusChangedEvent domainEvent, CancellationToken cancellationToken)
    {
        var newId = Guid.NewGuid();

        var json = JsonSerializer.Serialize(domainEvent);

        var integrationEvent = new OrderStatusUpdatedIntegrationEvent()
        {
            Id = newId,
            EventType = domainEvent.GetType().Name,
            CreatedAt = DateTime.UtcNow,
            Payload = json
        };
       
        await _publishEndpoint.Publish(integrationEvent, cancellationToken);

        _logger.LogInformation("Message sent. Id {Id}, OrderNumber {OrderNumber}, Status {Status}", 
            newId, domainEvent.OrderNumber, domainEvent.NewStatus);
    }
}
