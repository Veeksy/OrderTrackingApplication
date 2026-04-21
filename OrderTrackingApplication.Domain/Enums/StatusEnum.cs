namespace OrderTrackingApplication.Domain.Enums;

public enum StatusEnum : int
{
    created, //создан
    sent, //отправлен 
    delivered, //доставлен 
    canceled //отменен
}
