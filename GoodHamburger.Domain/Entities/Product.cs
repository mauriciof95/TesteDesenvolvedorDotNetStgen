using GoodHamburger.Domain.Enums;
using GoodHamburger.Domain.Exceptions;

namespace GoodHamburger.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; private set; }
    public decimal Price { get; private set; }
    public ProductType ProductType { get; private set; }

    public Product(string name, decimal price, ProductType productType)
    {
        Validate(name, price);

        Name = name;
        Price = price;
        ProductType = productType;
    }

    public void Update(string name, decimal price, ProductType productType)
    {
        Validate(name, price);

        Name = name;
        Price = price;
        ProductType = productType;
    }
    
    private void Validate(string name, decimal price)
    {
        if(string.IsNullOrEmpty(name))
            throw new ValidationException("Name is required");

        if(name.Length < 3)
            throw new ValidationException("Name must have at least 3 characters.");

        if(price <= 0)
            throw new ValidationException("Price must be greater than 0");
    }
}
