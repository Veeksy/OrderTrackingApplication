using MediatR;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Command.Add;

public record AddOrderCommand : IRequest<Model.Order>
{
    public required string Description { get; init; }
}
