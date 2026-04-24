using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Interfaces;
using GoodHamburger.Domain.Utils;
using GoodHamburger.Infrastructure.Context;

namespace GoodHamburger.Infrastructure.Repositories;

public class ProductRepository : BaseRepository<Product>, IProductRepository
{
    public ProductRepository(ApiDbContext context) : base(context)
    {
    }

    public override IQueryable<Product> ApplyPagedFilter(IQueryable<Product> query, BaseSearchParameters parameters)
    {
        if(!string.IsNullOrEmpty(parameters.SearchString))
            return query.Where(x => x.Name.Contains(parameters.SearchString));

        return query;
    }
}
