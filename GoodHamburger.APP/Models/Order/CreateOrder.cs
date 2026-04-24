using System;
using GoodHamburger.APP.Models.Product;

namespace GoodHamburger.APP.Models.Order;

public class CreateOrder
{
	public List<CreateOrderItem> Items { get; set; } = new();
}

public class CreateOrderItem
{
    public long ProductId { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public ProductType ProductType { get; set; }
    public decimal CurrentPrice { get; set; }
}
