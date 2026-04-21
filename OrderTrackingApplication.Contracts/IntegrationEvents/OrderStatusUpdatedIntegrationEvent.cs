namespace OrderTrackingApplication.Contracts.IntegrationEvents;

public record OrderStatusUpdatedIntegrationEvent
{
    public Guid Id { get; init; }
    public string EventType { get; init; } = string.Empty;
    public DateTime CreatedAt { get; init; }
    public required string Payload { get; init; }
}
