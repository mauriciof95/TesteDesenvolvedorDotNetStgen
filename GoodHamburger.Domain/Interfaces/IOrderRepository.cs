using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Interfaces;

public interface IOrderRepository : IRepository<Order>
{
    Task<Order> GetByIdWithItemsAsync(long id, CancellationToken cancellationToken);
    void DeleteOrderItens(List<OrderItem> items);
    void CreateOrderItems(List<OrderItem> items);
    void CreateOrderItem(OrderItem item);
}
