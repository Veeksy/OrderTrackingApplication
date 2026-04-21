using MediatR;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Query.GetOrder
{
    public record GetOrderQuery(long OrderNumber) : IRequest<Model.Order>;
}
