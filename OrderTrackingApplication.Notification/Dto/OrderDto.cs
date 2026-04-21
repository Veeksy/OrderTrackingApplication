using System.Text.Json.Serialization;

namespace OrderTrackingApplication.Notification.Dto;

public class OrderDto
{
    [JsonPropertyName("OrderNumber")]
    public long OrderNumber { get; set; }

    [JsonPropertyName("NewStatus")]
    public int Status { get; set; }
}
