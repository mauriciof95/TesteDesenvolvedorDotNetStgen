namespace GoodHamburger.APP.Models.Product;

public class Product
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductType ProductType { get; set; }
}
