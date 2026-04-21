using MassTransit;
using OrderTrackingApplication.Notification.Configuration;
using OrderTrackingApplication.Notification.Consumers;

namespace OrderTrackingApplication.Notification.Extensions;

public static class RabbitExtension
{
    public static void AddRabbit(this IServiceCollection services, IConfiguration configuration)
    {
        var rabbitSettings = configuration.GetSection(nameof(RabbitConfiguration)).Get<RabbitConfiguration>() ?? throw new InvalidOperationException("Нет конфигурации кролика");

        services.AddMassTransit(x =>
        {
            x.AddConsumer<OrderChangeStatusConsumer>();

            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitSettings.Host, rabbitSettings.Port, "/", h =>
                {
                    h.Username(rabbitSettings.Username);
                    h.Password(rabbitSettings.Password);
                });

                cfg.ReceiveEndpoint("notification-service", e =>
                {
                    e.Consumer<OrderChangeStatusConsumer>(context);
                });
            });
        });
    }
}
