namespace GoodHamburger.App.Utils
{
    public class BaseSearchParameters
    {
        public string SearchString { get; set; } = string.Empty;
        public string OrderType { get; set; } = "asc";
        public int PerPage { get; set; } = 50;
        public int CurrentPage { get; set; } = 0;
    }
}
