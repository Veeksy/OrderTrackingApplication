using OrderTrackingApplication.Domain.Enums;

namespace OrderTrackingApplication.Domain.Events;

public record OrderStatusChangedEvent(long OrderNumber, StatusEnum NewStatus);