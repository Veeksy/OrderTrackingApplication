using Microsoft.AspNetCore.SignalR;
using OrderTrackingApplication.Notification.Dto;
using OrderTrackingApplication.Notification.Hubs;

namespace OrderTrackingApplication.Notification.Services;

public class OrderUpdatedServices : IOrderUpdatedServices
{
    private readonly IHubContext<NotificationHub> _hubContext;

    public OrderUpdatedServices(IHubContext<NotificationHub> hubContext)
    {
        _hubContext = hubContext;
    }

    public async Task SendOrderStatusUpdateAsync(OrderDto order)
    {
        string groupName = NotificationHub.GetGroupName(order.OrderNumber.ToString());

        var orderStatus = new OrderStatusDto(order.OrderNumber, order.Status);

        await _hubContext.Clients.Group(groupName).SendAsync("ReceiveOrderStatus", orderStatus);
    }
}
