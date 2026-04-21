namespace OrderTrackingApplication.Infrastructure.RabbitSettings;

/// <summary>
/// Конфигурация для кролика
/// </summary>
public class RabbitConfiguration
{
    /// <summary>
    /// Хост
    /// </summary>
    public string Host { get; set; } = string.Empty;
    /// <summary>
    /// Порт
    /// </summary>
    public ushort Port { get; set; }
    /// <summary>
    /// Имя пользователя
    /// </summary>
    public string Username { get; set; } = string.Empty;
    /// <summary>
    /// Пароль
    /// </summary>
    public string Password { get; set; } = string.Empty;
}
