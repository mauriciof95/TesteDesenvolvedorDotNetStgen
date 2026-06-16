using GoodHamburger.Application.OrderContext;
using GoodHamburger.Application.OrderContext.DTOs;
using GoodHamburger.Domain.Entities;
using GoodHamburger.Domain.Utils;
using Microsoft.AspNetCore.Mvc;

namespace GoodHamburger.API.Controllers;

public class OrderController : GHControllerBase
{
    private readonly OrderService _orderServices;

    public OrderController(OrderService orderServices)
    {
        _orderServices = orderServices;
    }

    [HttpGet]
    public async Task<IActionResult> GetAllPaged(
        [FromQuery] BaseSearchParameters<Order> searchPrameters,
        CancellationToken cancellationToken)
    {
        return Ok(await _orderServices.SearchAsync(searchPrameters, cancellationToken));
    }

    [HttpPost]
    public async Task<IActionResult> Create(
        [FromBody] CreateOrderDTO dto,
        CancellationToken cancellationToken)
    {
        await _orderServices.CreateAsync(dto, cancellationToken);
        return Created();
    }

    [HttpGet("{id:long}")]
    public async Task<IActionResult> GetById(long id, CancellationToken cancellationToken)
    {
        return Ok(await _orderServices.GetByIdAsync(id, cancellationToken));
    }


    [HttpPut("{id:long}")]
    public async Task<IActionResult> Update([FromBody] UpdateOrderDTO dto, long id, CancellationToken cancellationToken)
    {
        await _orderServices.UpdateAsync(dto, id, cancellationToken);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    public async Task<IActionResult> Delete(long id, CancellationToken cancellationToken)
    {
        await _orderServices.DeleteAsync(id, cancellationToken);
        return NoContent();
    }
}
