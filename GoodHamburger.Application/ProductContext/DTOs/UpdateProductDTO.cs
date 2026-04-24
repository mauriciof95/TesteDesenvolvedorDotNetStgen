using GoodHamburger.Domain.Enums;

namespace GoodHamburger.Application.ProductContext.DTOs;

public class UpdateProductDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public decimal Price { get; set; }
    public ProductType ProductType { get; set; }
}
