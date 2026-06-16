using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Application.Utils;

public class PagedResultDTO<T, TEntity> where TEntity : BaseEntity
{
    public int TotalCount { get; set; }
    public ICollection<T> Rows { get; set; }
    public BaseSearchParameters<TEntity> SearchParameters { get; set; }
}
