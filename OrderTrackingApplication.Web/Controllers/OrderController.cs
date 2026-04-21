using MediatR;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using OrderTrackingApplication.Application.Common;
using OrderTrackingApplication.Application.UseCases.Order.Command.Add;
using OrderTrackingApplication.Application.UseCases.Order.Command.Update;
using OrderTrackingApplication.Application.UseCases.Order.Query.GetOrder;
using OrderTrackingApplication.Application.UseCases.Order.Query.GetOrderList;
using OrderTrackingApplication.Domain.Models;

namespace OrderTrackingApplication.Web.Controllers;

/// <summary>
/// Управление заказами
/// </summary>
/// <param name="sender"></param>
[ApiController]
[Route("[controller]")]
public class OrderController(ISender sender) : ControllerBase
{
    /// <summary>
    /// Получить заказ по Id
    /// </summary>
    /// <param name="id"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet("{id:long}", Name = nameof(GetOrderQuery))]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Ok<Order>> Get(long id, CancellationToken token)
    {
        var client = await sender.Send(new GetOrderQuery(id), token);

        return TypedResults.Ok(client);
    }

    /// <summary>
    /// Добавить заказ
    /// </summary>
    /// <param name="command"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPost]
    [ProducesResponseType(typeof(Order), StatusCodes.Status201Created)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Created<Order>> Create([FromBody] AddOrderCommand command, CancellationToken token)
    {
        var order = await sender.Send(command, token);

        return TypedResults.Created($"/{nameof(Order)}/{order.OrderNumber}", order);
    }

    /// <summary>
    /// Получить список заказов
    /// </summary>
    /// <param name="command"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpGet]
    [ProducesResponseType(typeof(PaginatedList<Order>), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Ok<PaginatedList<Order>>> GetList([FromQuery] GetOrderListQuery command, CancellationToken token)
    {
        var clientList = await sender.Send(command, token);

        return TypedResults.Ok(clientList);
    }

    /// <summary>
    /// Обновить заказ, его статус
    /// </summary>
    /// <param name="command"></param>
    /// <param name="token"></param>
    /// <returns></returns>
    [HttpPut]
    [ProducesResponseType(typeof(Order), StatusCodes.Status200OK)]
    [ProducesResponseType(StatusCodes.Status400BadRequest)]
    public async Task<Ok<Order>> Update([FromBody] UpdateOrderCommand command, CancellationToken token)
    {
        var order = await sender.Send(command, token);

        return TypedResults.Ok(order);
    }
}
