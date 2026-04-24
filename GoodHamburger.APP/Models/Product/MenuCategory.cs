namespace GoodHamburger.APP.Models.Product;

public class MenuItem
{
    public long Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Price { get; set; } = string.Empty;
}

public class MenuCategory
{
    public string Group { get; set; } = string.Empty;
    public List<MenuItem> Items { get; set; } = new();
}