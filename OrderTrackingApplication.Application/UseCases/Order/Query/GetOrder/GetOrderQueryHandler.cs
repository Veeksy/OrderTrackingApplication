using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderTrackingApplication.Application.Common;
using OrderTrackingApplication.Application.Exceptions;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Query.GetOrder;

public class GetOrderQueryHandler : IRequestHandler<GetOrderQuery, Model.Order>
{
    private readonly IApplicationDbContext _context;

    public GetOrderQueryHandler(IApplicationDbContext context) => _context = context;
    
    public async Task<Model.Order> Handle(GetOrderQuery request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.AsNoTracking().FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

        if (order is null)
            throw new NotFoundException(request.OrderNumber);

        return order;
    }
}
