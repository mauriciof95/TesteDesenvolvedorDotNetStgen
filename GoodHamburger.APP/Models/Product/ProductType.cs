using System.ComponentModel;

namespace GoodHamburger.APP.Models.Product;

public enum ProductType
{
    [Description("Sanduíche")]
    Sandwich = 0,

    [Description("Batata Frita")]
    Fries = 1,

    [Description("Bebida")]
    Drink = 2
}
