using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.OrderContext.DTOs;

public class OrderItemDTO
{
    public long Id { get; set; }
    public string ProductName { get; set; }
    public ProductType ProductType { get; set; }
    public decimal CurrentPrice { get; set; }
    public long ProductId { get; set; }

    public OrderItemDTO(OrderItem orderItem)
    {
        Id = orderItem.Id;
        ProductName = orderItem.ProductName;
        ProductType = orderItem.ProductType;
        CurrentPrice = orderItem.CurrentPrice;
        ProductId = orderItem.ProductId;
    }

    public static OrderItemDTO[] ToDTO(List<OrderItem> orders)
    {
        return orders
                .Select(p => new OrderItemDTO(p))
                .ToArray();
    }
}
