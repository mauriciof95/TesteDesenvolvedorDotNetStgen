using GoodHamburger.Domain.Entities;

namespace GoodHamburger.Application.ProductContext.DTOs;

public class MenuItemDTO
{
    public long Id { get; set; }
    public string Name { get; set; }
    public string Price { get; set; }

    public MenuItemDTO(Product product)
    {
        Id = product.Id;
        Name = product.Name;
        Price = product.Price.ToString("C2");
    }
}
