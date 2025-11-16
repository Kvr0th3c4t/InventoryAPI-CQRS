using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Dtos.MovimientoStockDtos;
using MediatR;
using InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;
using InventoryAPI.Features.MovimientosStock.Queries.GetMovimientoById;
using InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MovimientosStockController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimientosStockController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllMovimientosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetMovimientoById(int id)
    {
        var query = new GetMovimientoByIdQuery(id);

        var result = await _mediator.Send(query);

        return Ok(result);
    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateMovimientoStockDto dto)
    {
        var command = new CreateMovimientoStockCommand(
            dto.ProductoId,
            dto.ProveedorId,
            dto.Tipo,
            dto.Cantidad,
            dto.Razon
        );

        var result = await _mediator.Send(command);

        return CreatedAtAction(nameof(GetMovimientoById), new { id = result.Id }, result);
    }

}