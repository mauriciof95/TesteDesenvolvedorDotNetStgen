using System.Linq.Expressions;
using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Domain.Utils;

public class BaseSearchParameters<TEntity> where TEntity : BaseEntity
{
    public string? SearchString { get; set; }
    public string OrderType { get; set; } = "asc";
    public int PerPage { get; set; } = 50;
    public int CurrentPage { get; set; } = 0;
    public Expression<Func<TEntity, object>> OrderBy { get; set; } = (x => x.Id);
}
