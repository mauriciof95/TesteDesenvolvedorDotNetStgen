namespace GoodHamburger.Domain.Utils;

public class PagedQueryResult<T>
{
    public int TotalCount { get; set; }
    public List<T> Rows { get; set; }
}

