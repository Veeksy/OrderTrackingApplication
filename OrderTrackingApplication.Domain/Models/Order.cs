using OrderTrackingApplication.Domain.Enums;
using OrderTrackingApplication.Domain.Events;

namespace OrderTrackingApplication.Domain.Models;

/// <summary>
/// Заказ
/// </summary>
public record Order
{
    /// <summary>
    /// Номер заказа
    /// </summary>
    public long OrderNumber { get; init; }
    /// <summary>
    /// Описание
    /// </summary>
    public required string Description { get; init; }
    /// <summary>
    /// Дата создания
    /// </summary>
    public DateTime CreatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Статус заказа
    /// </summary>
    public StatusEnum Status { get; private set; } = StatusEnum.created;
    /// <summary>
    /// Дата обновления
    /// </summary>
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;

    /// <summary>
    /// Событие смены статуса
    /// </summary>
    public OrderStatusChangedEvent? OrderStatusChangedEvent { get; private set; }

    /// <summary>
    /// Сменить статус
    /// </summary>
    /// <param name="status">Список статуса в StatusEnum</param>
    /// <exception cref="InvalidOperationException">Доставленный заказ менять нельзя</exception>
    public void ChangeStatus(StatusEnum status)
    {
        if (Status == StatusEnum.delivered)
            throw new InvalidOperationException("Доставленный заказ менять нельзя");

        Status = status;
        UpdatedAt = DateTime.UtcNow;

        OrderStatusChangedEvent = new OrderStatusChangedEvent(OrderNumber, status);
    }
}
