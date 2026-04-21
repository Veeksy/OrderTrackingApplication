using Microsoft.AspNetCore.SignalR;

namespace OrderTrackingApplication.Notification.Hubs;

public class NotificationHub : Hub
{
    public async Task SubscribeToOrder(string orderNumber)
    {
        await Groups.AddToGroupAsync(Context.ConnectionId, GetGroupName(orderNumber));
    }

    public async Task UnsubscribeFromOrder(string orderNumber)
    {
        await Groups.RemoveFromGroupAsync(Context.ConnectionId, GetGroupName(orderNumber));
    }

    public static string GetGroupName(string orderNumber) => $"order-{orderNumber}";
}
