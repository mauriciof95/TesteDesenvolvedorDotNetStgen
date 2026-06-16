using System.Threading.Tasks;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Utils;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Extensions;

public static class QueryExtensions
{
    public static async Task<PagedQueryResult<TEntity>> PaginateAsync<TEntity>(
        this IQueryable<TEntity> query, 
        BaseSearchParameters<TEntity> parameters,
        CancellationToken cancellationToken = default
    )
        where TEntity: BaseEntity
    {
        var pagedResult = new PagedQueryResult<TEntity>();

        pagedResult.TotalCount = await query.CountAsync(cancellationToken);

        query = parameters.OrderType == "asc"
            ? query.OrderBy(parameters.OrderBy)
            : query.OrderByDescending(parameters.OrderBy);
        
        query = query.Skip(parameters.PerPage * parameters.CurrentPage).Take(parameters.PerPage);

        pagedResult.Rows = await query.ToListAsync(cancellationToken);

        return pagedResult;
    }
}