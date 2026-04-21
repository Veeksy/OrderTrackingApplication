namespace OrderTrackingApplication.Notification.Dto;

public class OrderStatusDto
{
    public long orderNumber { get; private set; }
    public int status { get; private set; }
    public DateTime updatedAt { get; private set; }

    public OrderStatusDto(long orderNumber, int status)
    {
        this.orderNumber = orderNumber;
        this.status = status;
        updatedAt = DateTime.UtcNow;
    }
}
