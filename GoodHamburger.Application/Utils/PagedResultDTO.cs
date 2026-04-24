using GoodHamburger.Domain.Utils;

namespace GoodHamburger.Application.Utils;

public class PagedResultDTO<T>
{
    public int TotalCount { get; set; }
    public ICollection<T> Rows { get; set; }
    public BaseSearchParameters SearchParameters { get; set; }
}
