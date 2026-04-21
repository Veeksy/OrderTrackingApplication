using MediatR;
using OrderTrackingApplication.Application.Common;
using Model = OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Application.UseCases.Order.Command.Add;


// Можно ещё добавить валидатор из FluentValidation, но здесь overhead.
// Поэтому отказался от этой идеии 

public class AddOrderCommandHandler : IRequestHandler<AddOrderCommand, Model.Order>
{
    private readonly IApplicationDbContext _context;

    public AddOrderCommandHandler(IApplicationDbContext context) => _context = context;

    public async Task<Model.Order> Handle(AddOrderCommand request, CancellationToken cancellationToken)
    {
        var newOrder = new Model.Order()
        {
            Description = request.Description
        };

        _context.Orders.Add(newOrder);
        
        await _context.SaveChangesAsync(cancellationToken);

        return newOrder;
    }
}
