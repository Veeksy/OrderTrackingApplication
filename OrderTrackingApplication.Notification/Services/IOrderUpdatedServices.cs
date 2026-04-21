using OrderTrackingApplication.Notification.Dto;

namespace OrderTrackingApplication.Notification.Services;

public interface IOrderUpdatedServices
{
    public Task SendOrderStatusUpdateAsync(OrderDto order);
}
