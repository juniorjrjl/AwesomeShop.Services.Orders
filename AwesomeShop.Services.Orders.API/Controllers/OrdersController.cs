using AwesomeShop.Services.Orders.Application.Commands;
using AwesomeShop.Services.Orders.Application.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeShop.Services.Orders.API.Controllers;

[Route("api/orders")]
public class OrdersController(IMediator mediator) : Controller
{

    private readonly IMediator _mediator = mediator;

    [HttpGet("{id}")]
    public async Task<IActionResult> Get(Guid id)
    {
        var query = new GetOrderById(id);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpPost]
    public async Task<IActionResult> Post([FromBody] AddOrder command)
    {
        var id = await _mediator.Send(command);
        return CreatedAtAction(nameof(Get), new { id}, command);
    }

}
