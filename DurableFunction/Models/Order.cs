namespace DurableFunction.Models;

public enum OrderState
{
    Created,
    Paid,
    Delivered,
    Invoiced
}

public class Order
{
    public Guid Id { get; private set; }
    public OrderState State { get; set; }

    public Order()
    {
        Id = Guid.NewGuid();
    }
}
