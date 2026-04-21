using OrderTrackingApplication.Domain.Events;

namespace OrderTrackingApplication.Application.EventsHandlers.Order.OrderUpdate;

public interface IOrderUpdateEventHandler
{
    Task HandleOrderStatusChangedAsync(OrderStatusChangedEvent domainEvent, CancellationToken cancellationToken);
}
