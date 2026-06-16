using System.Threading.Tasks;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;
using GoodHamburger.Extensions;
using GoodHamburger.Infrastructure.Context;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApiDbContext context) : base(context)
    {
    }

    public override async Task<PagedQueryResult<Product>> GetPagedResultAsync(BaseSearchParameters<Product> parameters, CancellationToken cancellationToken)
    {
        var result = await _db.AsNoTracking()
            .Where(x => string.IsNullOrEmpty(parameters.SearchString) || x.Name.Contains(parameters.SearchString))
            .PaginateAsync(parameters, cancellationToken);

        return result;
    }
}
