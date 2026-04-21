using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Infrastructure.Configuration;

public class OrderConfiguration : IEntityTypeConfiguration<Order>
{
    public void Configure(EntityTypeBuilder<Order> builder)
    {
        builder.HasKey(x => x.OrderNumber);

        builder.Property(x => x.OrderNumber)
            .ValueGeneratedOnAdd();

        builder.Property(x => x.Description)
            .HasMaxLength(1500)
            .IsRequired();

        builder.Property(x => x.CreatedAt)
            .HasColumnType("timestamp with time zone"); 

        builder.Property(x => x.UpdatedAt)
            .HasColumnType("timestamp with time zone");

        builder.Ignore(e => e.OrderStatusChangedEvent);
    }
}
