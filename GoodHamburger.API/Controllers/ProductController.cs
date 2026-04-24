using GoodHamburger.Application.ProductContext;
using GoodHamburger.Application.ProductContext.DTOs;
using GoodHamburger.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

public class ProductController : GHControllerBase
{
    private readonly ProductService _productServices;

    public ProductController(ProductService productServices)
    {
        _productServices = productServices;
    }

    [HttpGet("menu")]
    public IActionResult GetMenu()
    {
        var result = _productServices.GetMenu();
        return Ok(result);
    }

    [HttpGet("getall")]
    public IActionResult GetAll()
    {
        return Ok(_productServices.GetAll());
    }

    [HttpGet]
    public IActionResult GetAllPaged([FromQuery] BaseSearchParameters searchPrameters)
    {
        return Ok(_productServices.Search(searchPrameters));
    }

    [HttpGet("{id:long}")]
    public IActionResult GetById(long id)
    {
        return Ok(_productServices.GetById(id));
    }


    [HttpPost]
    public IActionResult Create([FromBody] CreateProductDTO dto)
    {
        _productServices.Create(dto);
        return Created();
    }

    [HttpPut("{id:long}")]
    public IActionResult Update([FromBody] UpdateProductDTO dto, long id)
    {
        _productServices.Update(dto, id);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        _productServices.Delete(id);
        return NoContent();
    }
}
