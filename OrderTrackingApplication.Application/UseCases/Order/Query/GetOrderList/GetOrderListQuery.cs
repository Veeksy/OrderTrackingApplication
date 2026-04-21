using MediatR;
using OrderTrackingApplication.Application.Common;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Query.GetOrderList;

public record GetOrderListQuery : IRequest<PaginatedList<Model.Order>>
{
    public long? SearchOrderNumber { get; init; }
    public string? SearchDescription { get; init; }

    public DateTime? SearchCreatedAt { get; init; }
    public DateTime? SearchUpdatedAt { get; init; }

    public int PageNumber { get; init; } = 1;
    public int PageSize { get; init; } = 10;
}
