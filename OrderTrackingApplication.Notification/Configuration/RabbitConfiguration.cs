namespace OrderTrackingApplication.Notification.Configuration;

public class RabbitConfiguration
{
    public string Host { get; set; } = string.Empty;
    public ushort Port { get; set; } 
    public string Username { get; set; } = string.Empty;
    public string Password { get; set; } = string.Empty;
}
