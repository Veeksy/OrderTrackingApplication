using MediatR;
using Microsoft.EntityFrameworkCore;
using OrderTrackingApplication.Application.Common;
using OrderTrackingApplication.Application.EventsHandlers.Order.OrderUpdate;
using OrderTrackingApplication.Application.Exceptions;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Command.Update;

public class UpdateOrderCommandHandler : IRequestHandler<UpdateOrderCommand, Model.Order>
{
    private readonly IApplicationDbContext _context;
    private readonly IOrderUpdateEventHandler _orderUpdateEvent;

    public UpdateOrderCommandHandler(IApplicationDbContext context, IOrderUpdateEventHandler orderUpdateEvent)
    {
        _context = context;
        _orderUpdateEvent = orderUpdateEvent;
    }

    public async Task<Model.Order> Handle(UpdateOrderCommand request, CancellationToken cancellationToken)
    {
        var order = await _context.Orders.FirstOrDefaultAsync(x => x.OrderNumber == request.OrderNumber, cancellationToken);

        if (order is null)
            throw new NotFoundException(request.OrderNumber);


        using var transaction = await _context.BeginTransactionAsync(cancellationToken);

		try
		{
            order.ChangeStatus(request.Status);

            await _context.SaveChangesAsync(cancellationToken);

            await _orderUpdateEvent.HandleOrderStatusChangedAsync(order.OrderStatusChangedEvent!, cancellationToken);

            await transaction.CommitAsync(cancellationToken);

            return order;
		}
		catch (Exception ex)
		{
            await transaction.RollbackAsync(cancellationToken);

			throw new BusinessException("Произошла ошибка во время обновления заказа", ex);
		}
    }
}
