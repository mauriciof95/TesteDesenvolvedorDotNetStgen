using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.ProductContext.DTOs;

public class ProductDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductType ProductType { get; set; }

    public ProductDTO (Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Price = product.Price;
        ProductType = product.ProductType;
    }

    public static ProductDTO[] ToDTO(List<Product> products)
    {
        return products
                .Select(p => new ProductDTO(p))
                .ToArray();
    }
}
