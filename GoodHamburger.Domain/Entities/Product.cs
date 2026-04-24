using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductType ProductType { get; set; }
}
