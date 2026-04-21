using MediatR;
using Microsoft.Extensions.DependencyInjection;
using OrderTrackingApplication.Application.Behaviors;
using OrderTrackingApplication.Application.EventsHandlers.Order.OrderUpdate;
using System.Reflection;

namespace OrderTrackingApplication.Application;

public static class AddApplicationDI
{
    public static void AddApplication(this IServiceCollection services)
    {
        var assembly = Assembly.GetExecutingAssembly();

        services.AddScoped<IOrderUpdateEventHandler, OrderUpdateEventHandler>();

        services
            .AddMediatR(cfg => cfg
                .AddBehavior(typeof(IPipelineBehavior<,>), typeof(ExceptionHandlingBehavior<,>))
                .AddBehavior(typeof(IPipelineBehavior<,>), typeof(LoggingBehavior<,>))
                .RegisterServicesFromAssembly(assembly));
    }
}
