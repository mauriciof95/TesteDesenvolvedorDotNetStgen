using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class OrderItem : BaseEntity
{
    public long OrderId { get; set; }
    public long ProductId { get; set; }
    public string ProductName { get; set; }
    public ProductType ProductType { get; set; }
    public decimal CurrentPrice { get; set; }
    public Order Order { get; set; }
    public Product Product { get; set; }
}
