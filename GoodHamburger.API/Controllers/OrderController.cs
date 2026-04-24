using GoodHamburger.Application.OrderContext;
using GoodHamburger.Application.OrderContext.DTOs;
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
    public IActionResult GetAllPaged([FromQuery] BaseSearchParameters searchPrameters)
    {
        return Ok(_orderServices.Search(searchPrameters));
    }

    [HttpPost]
    public IActionResult Create([FromBody] CreateOrderDTO dto)
    {
        _orderServices.Create(dto);
        return Created();
    }

    [HttpGet("{id:long}")]
    public IActionResult GetById(long id)
    {
        return Ok(_orderServices.GetById(id));
    }


    [HttpPut("{id:long}")]
    public IActionResult Update([FromBody] UpdateOrderDTO dto, long id)
    {
        _orderServices.Update(dto, id);
        return Ok();
    }

    [HttpDelete("{id:long}")]
    public IActionResult Delete(long id)
    {
        _orderServices.Delete(id);
        return NoContent();
    }
}
