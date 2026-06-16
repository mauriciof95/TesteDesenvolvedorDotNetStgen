using System.Threading.Tasks;
using GoodHamburger.Application.ProductContext;
using GoodHamburger.Application.ProductContext.DTOs;
using GoodHamburger.Domain.Entities;
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
    public async Task<IActionResult> GetMenu()
    {
        var result = await _productServices.GetMenuAsync();
        return Ok(result);
    }

    [HttpGet("getall")]
    public async Task<IActionResult> GetAll(CancellationToken cancellationToken)
    {
        return Ok(await _productServices.GetAllAsync(cancellationToken));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPaged(
        [FromQuery] BaseSearchParameters<Product> searchPrameters,
        CancellationToken cancellationToken)
    {
        return Ok(await _productServices.SearchAsync(searchPrameters, cancellationToken));
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id)
    {
        return Ok(await _productServices.GetByIdAsync(id));
    }


    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductDTO dto)
    {
        await _productServices.CreateAsync(dto);
        return Created();
    }

    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update([FromBody] UpdateProductDTO dto, long id)
    {
        await _productServices.UpdateAsync(dto, id);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id)
    {
        await _productServices.DeleteAsync(id);
        return NoContent();
    }
}
