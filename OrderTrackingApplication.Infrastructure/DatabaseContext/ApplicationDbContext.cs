using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderTrackingApplication.Application.Common;
using OrderTrackingApplication.Domain.Models;
using System.Reflection;

namespace OrderTrackingApplication.Infrastructure.DatabaseContext;

public class ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : DbContext(options), IApplicationDbContext
{
    public DbSet<Order> Orders { get; set; }

    public async Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken)
    {
        return await Database.BeginTransactionAsync(cancellationToken);
    }

    protected override void OnModelCreating(ModelBuilder optionsBuilder)
    {
        optionsBuilder.ApplyConfigurationsFromAssembly(Assembly.GetExecutingAssembly());
    }
}
