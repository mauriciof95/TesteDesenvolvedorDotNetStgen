using GoodHamburger.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace GoodHamburger.Infrastructure.Seeds;

internal class SeedConfig
{
    public void ApplySeeds(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Product>().HasData(new ProductSeed().Seeds());
    }
}
