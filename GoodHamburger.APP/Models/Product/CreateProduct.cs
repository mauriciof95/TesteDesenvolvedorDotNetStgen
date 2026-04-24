using System.ComponentModel.DataAnnotations;

namespace GoodHamburger.APP.Models.Product;

public class CreateProduct
{
    [Required]
    [MinLength(3)]
    public string Name { get; set; }

    [Required]
    public decimal Price { get; set; }

    [Required]
    public ProductType ProductType { get; set; }
}
