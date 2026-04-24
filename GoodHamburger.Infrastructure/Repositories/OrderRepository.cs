using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;
using GoodHamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public class OrderRepository : BaseRepository<Order>, IOrderRepository
{
    protected readonly DbSet<OrderItem> _dbOrderItem;

    public OrderRepository(ApiDbContext context) : base(context)
    {
        _dbOrderItem = context.Set<OrderItem>();
    }

    public override IQueryable<Order> ApplyPagedFilter(IQueryable<Order> query, BaseSearchParameters parameters)
    {
        if (!string.IsNullOrEmpty(parameters.SearchString))
            return query.Where(x => x.Id.ToString() == parameters.SearchString);

        return query;
    }

    public override IQueryable<Order> ApplyIncludes(IQueryable<Order> query)
    {
        query = query.Include(x => x.OrderItems.Where(i => i.DeletedAt == null));
        return query;
    }

    public Order GetByIdWithItems(long id)
    {
        return _db.Include(x => x.OrderItems.Where(i => i.DeletedAt == null))
            .FirstOrDefault(x => x.Id == id);
    }

    public void DeleteOrderItens(List<OrderItem> items)
        => _dbOrderItem.RemoveRange(items);

    public void CreateOrderItems(List<OrderItem> items)
        => _dbOrderItem.AddRange(items);

    public void CreateOrderItem(OrderItem item)
        => _dbOrderItem.Add(item);
}
