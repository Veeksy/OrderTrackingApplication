using MassTransit;
using OrderTrackingApplication.Contracts.IntegrationEvents;
using OrderTrackingApplication.Notification.Dto;
using OrderTrackingApplication.Notification.Services;
using System.Text.Json;

namespace OrderTrackingApplication.Notification.Consumers;

public class OrderChangeStatusConsumer : IConsumer<OrderStatusUpdatedIntegrationEvent>
{
    private readonly ILogger<OrderChangeStatusConsumer> _logger;
    private readonly IOrderUpdatedServices _orderUpdatedServices;

    public OrderChangeStatusConsumer(ILogger<OrderChangeStatusConsumer> logger, IOrderUpdatedServices orderUpdatedServices)
    {
        _logger = logger;
        _orderUpdatedServices = orderUpdatedServices;
    }

    public async Task Consume(ConsumeContext<OrderStatusUpdatedIntegrationEvent> context)
    {
        var message = context.Message;

        try
        {
            _logger.LogInformation("Принято сообщение {MessageId}. Тип: {EventType}, Время: {Timestamp}",
                message.Id, message.EventType, DateTime.UtcNow);

            if (!string.IsNullOrEmpty(message.Payload))
            {
                var orderDto = JsonSerializer.Deserialize<OrderDto>(message.Payload)!;

                _logger.LogInformation("Заказ {OrderNumber} сменил статус на {NewStatus}",
                    orderDto.OrderNumber, orderDto.Status);

                await _orderUpdatedServices.SendOrderStatusUpdateAsync(orderDto);
            }
            else
            {
                _logger.LogWarning("Пустой payload в сообщениий: {MessageId}",
                    message.Id);
            }
            
            _logger.LogInformation("Сообщение {MessageId} успешно обработано", message.Id);

            await context.ConsumeCompleted;
        }
        catch (Exception ex)
        {
            _logger.LogError(ex,
                "Ошибка при обработке сообщения {MessageId}. Тип сообщения: {MessageType}",
                message.Id, message.EventType);

            throw;
        }
    }
}
