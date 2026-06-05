using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Infrastructure.Seeds;

internal class ProductSeed
{
    public Product[] Seeds()
    {
        var date = new DateTime(2026, 04, 21, 00, 00, 00, DateTimeKind.Utc);

        return new Product[]
        {
            new Product("X Burger", 5.00m, ProductType.Sandwich).SetCreatedAt(date) as Product,
            new Product("X Egg", 4.50m, ProductType.Sandwich).SetCreatedAt(date) as Product,
            new Product("X Bacon", 7.00m, ProductType.Sandwich).SetCreatedAt(date) as Product,
            new Product("Batata frita", 2.00m, ProductType.Fries).SetCreatedAt(date) as Product,
            new Product("Refrigerante", 2.50m, ProductType.Drink).SetCreatedAt(date) as Product,
        };
    }
}
