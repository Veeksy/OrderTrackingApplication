using MediatR;
using OrderTrackingApplication.Domain.Enums;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Command.Update;

public record UpdateOrderCommand : IRequest<Model.Order>
{
    public long OrderNumber { get; init; }

    public StatusEnum Status { get; init; }
}
