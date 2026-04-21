using MediatR;
using OrderTrackingApplication.Application.Common;
using Model = OrderTrackingApplication.Domain.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace OrderTrackingApplication.Application.UseCases.Order.Query.GetOrderList
{
    public class GetOrderListQueryHandler : IRequestHandler<GetOrderListQuery, PaginatedList<Model.Order>>
    {
        private readonly IApplicationDbContext _context;

        public GetOrderListQueryHandler(IApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<PaginatedList<Model.Order>> Handle(GetOrderListQuery request, CancellationToken cancellationToken)
        {
            var query = _context.Orders.AsQueryable();

            if (!string.IsNullOrEmpty(request.SearchDescription))
                query.Where(x => x.Description.Contains(request.SearchDescription, StringComparison.InvariantCultureIgnoreCase));

            if (request.SearchOrderNumber is not null)
                query.Where(x => x.OrderNumber == request.SearchOrderNumber);

            if (request.SearchCreatedAt is not null)
                query.Where(x => x.CreatedAt >= request.SearchCreatedAt);

            if (request.SearchUpdatedAt is not null)
                query.Where(x => x.UpdatedAt >= request.SearchUpdatedAt);

            return await query
               .Distinct().PaginatedListAsync(request.PageNumber, request.PageSize);
        }
    }
}
