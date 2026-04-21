using OrderTrackingApplication.Notification.Extensions;
using OrderTrackingApplication.Notification.Hubs;
using OrderTrackingApplication.Notification.Services;

namespace OrderTrackingApplication.Notification;

public class Program
{
    public static void Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.

        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle
        builder.Services.AddEndpointsApiExplorer();
        builder.Services.AddSwaggerGen();

        builder.Services.AddCors(options =>
        {
            options.AddPolicy("AllowFrontend", policy =>
            {
                policy.WithOrigins(
                        "http://localhost:5173",
                        "http://127.0.0.1:5173",
                        "http://localhost:3000",
                        "http://127.0.0.1:3000"
                    )
                    .AllowAnyHeader()
                    .AllowAnyMethod()
                    .AllowCredentials();
            });
        });

        builder.Services.AddRabbit(builder.Configuration);

        builder.Services.AddScoped<IOrderUpdatedServices, OrderUpdatedServices>();

        builder.Services.AddSignalR();

        var app = builder.Build();

        app.UseCors("AllowFrontend");

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        //app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapHub<NotificationHub>("/hubs/notification");

        app.MapControllers();

        app.Run();
    }
}
