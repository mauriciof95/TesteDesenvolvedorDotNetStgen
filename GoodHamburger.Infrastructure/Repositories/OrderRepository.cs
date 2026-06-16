using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;
using GoodHamburger.Extensions;
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

    public override async Task<PagedQueryResult<Order>> GetPagedResultAsync(BaseSearchParameters<Order> parameters, CancellationToken cancellationToken)
    {
        var result = await _db.AsNoTracking()
            .Where(x => string.IsNullOrEmpty(parameters.SearchString) || x.Id.ToString() == parameters.SearchString)
            .PaginateAsync(parameters, cancellationToken);

        return result;
    }

    public async Task<Order> GetByIdWithItemsAsync(long id, CancellationToken cancellationToken)
        => await _db.Include(x => x.OrderItems).FirstOrDefaultAsync(x => x.Id == id, cancellationToken);

    public void DeleteOrderItens(List<OrderItem> items)
        => _dbOrderItem.RemoveRange(items);

    public void CreateOrderItems(List<OrderItem> items)
        => _dbOrderItem.AddRange(items);

    public void CreateOrderItem(OrderItem item)
        => _dbOrderItem.Add(item);
}
