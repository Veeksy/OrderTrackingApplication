using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.Common;

public interface IApplicationDbContext
{
    DbSet<Order> Orders { get; }

    Task<IDbContextTransaction> BeginTransactionAsync(CancellationToken cancellationToken);

    Task<int> SaveChangesAsync(CancellationToken cancellationToken);
}
