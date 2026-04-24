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
            new Product { Id = 1, Name = "X Burger", Price = 5.00m, ProductType = ProductType.Sandwich, CreatedAt = date, UpdatedAt = date },
            new Product { Id = 2, Name = "X Egg", Price = 4.50m, ProductType = ProductType.Sandwich, CreatedAt = date, UpdatedAt = date  },
            new Product { Id = 3, Name = "X Bacon", Price = 7.00m, ProductType = ProductType.Sandwich, CreatedAt = date, UpdatedAt = date  },
            new Product { Id = 4, Name = "Batata frita", Price = 2.00m, ProductType = ProductType.Fries, CreatedAt = date, UpdatedAt = date  },
            new Product { Id = 5, Name = "Refrigerante", Price = 2.50m, ProductType = ProductType.Drink, CreatedAt = date, UpdatedAt = date  },
        };
    }
}
