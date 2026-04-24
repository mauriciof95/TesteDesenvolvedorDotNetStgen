using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.OrderContext.DTOs;

public class OrderDTO
{
    public long Id { get; set; }
    public decimal Subtotal { get; set; }
    public decimal Discount { get; set; }
    public decimal Total { get; set; }

    public OrderItemDTO[] OrderItems { get; set; }

    public OrderDTO(Order order)
    {
        Id = order.Id;
        Subtotal = order.Subtotal;
        Discount = order.Discount;
        Total = order.Total;

        OrderItems = order.OrderItems != null && order.OrderItems.Any()
            ? OrderItemDTO.ToDTO(order.OrderItems)
            : null;
    }

    public static ICollection<OrderDTO> ToDTO(List<Order> orders)
    {
        return orders
                .Select(p => new OrderDTO(p))
                .ToList();
    }
}
