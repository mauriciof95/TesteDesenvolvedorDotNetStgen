using GoodHamburger.APP.Models.Product;

namespace GoodHamburger.APP.Models.Order;

public class OrderItem
{
    public long Id { get; set; }
	public long ProductId { get; set; }
    public string ProductName { get; set; }
    public ProductType ProductType { get; set; }
    public decimal CurrentPrice { get; set; }
}

public class Order
{
    public long Id { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }
    public List<OrderItem> OrderItems { get; set; }
}