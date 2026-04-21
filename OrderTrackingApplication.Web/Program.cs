using OrderTrackingApplication.Infrastructure;
using OrderTrackingApplication.Application;
using System.Reflection;
using Microsoft.OpenApi.Models;
using OrderTrackingApplication.Infrastructure.DatabaseContext;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackingApplication.Web;

#pragma warning disable CS1591 // Отсутствует комментарий XML для открытого видимого типа или члена
public class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        // Add services to the container.
            
        builder.Services.AddControllers();
        // Learn more about configuring Swagger/OpenAPI at https://aka.ms/aspnetcore/swashbuckle

        builder.Services.AddInfrastructure(builder.Configuration);

        builder.Services.AddApplication();

        builder.Services.AddEndpointsApiExplorer();

        builder.Services.AddCors(options =>
        {
            options.AddDefaultPolicy(policy =>
            {
                policy.AllowAnyOrigin().AllowAnyHeader().AllowAnyMethod();
            });
        });

        var basePath = AppDomain.CurrentDomain.BaseDirectory;

        builder.Services.AddSwaggerGen(c =>
        {
            c.SwaggerDoc("v1", new OpenApiInfo { Title = "Order API", Version = "v1" });

            var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
            var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
            c.IncludeXmlComments(xmlPath);
        });

        var app = builder.Build();

        using (var scope = app.Services.CreateScope())
        {
            var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
            var logger = scope.ServiceProvider.GetRequiredService<ILogger<Program>>();

            var retryCount = 0;
            const int maxRetries = 10;

            while (retryCount < maxRetries)
            {
                try
                {
                    logger.LogInformation("Applying migrations...");

                    await dbContext.Database.MigrateAsync();

                    logger.LogInformation("Database schema is ready");
                    break;
                }
                catch (Exception ex)
                {
                    retryCount++;

                    if (retryCount == maxRetries)
                    {
                        logger.LogError(ex, "Failed to apply migrations");
                        throw;
                    }

                    logger.LogWarning(ex, "Database not ready. Retrying in 5 seconds...");
                    await Task.Delay(5000);
                }
            }
        }

        app.UseCors();

        // Configure the HTTP request pipeline.
        if (app.Environment.IsDevelopment())
        {
            app.UseSwagger();
            app.UseSwaggerUI();
        }

        app.UseHttpsRedirection();

        app.UseAuthorization();

        app.MapControllers();

        app.Run();
    }
}
