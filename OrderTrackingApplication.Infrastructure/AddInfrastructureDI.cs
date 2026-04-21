using MassTransit;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderTrackingApplication.Application.Common;
using OrderTrackingApplication.Infrastructure.DatabaseContext;
using OrderTrackingApplication.Infrastructure.RabbitSettings;

namespace OrderTrackingApplication.Infrastructure;

public static class AddInfrastructureDI
{
    public static void AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<IApplicationDbContext, ApplicationDbContext>(options => options
            .UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        var rabbitSettings = configuration.GetSection(nameof(RabbitConfiguration)).Get<RabbitConfiguration>() ?? throw new InvalidOperationException("Нет конфигурации кролика");

        services.AddMassTransit(x =>
        {
            x.UsingRabbitMq((context, cfg) =>
            {
                cfg.Host(rabbitSettings.Host, rabbitSettings.Port, "/", h =>
                {
                    h.Username(rabbitSettings.Username);
                    h.Password(rabbitSettings.Password);
                });
            });
        });
    }
}
