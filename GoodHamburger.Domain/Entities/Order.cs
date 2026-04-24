namespace GoodHamburger.Domain.Entities;

public class Order : BaseEntity
{
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    public List<OrderItem> OrderItems { get; set; }
}
