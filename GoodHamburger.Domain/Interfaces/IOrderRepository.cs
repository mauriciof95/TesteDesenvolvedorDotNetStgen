using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    public Order GetByIdWithItems(long id);
    public void DeleteOrderItens(List<OrderItem> items);
    public void CreateOrderItems(List<OrderItem> items);
    public void CreateOrderItem(OrderItem item);
}
