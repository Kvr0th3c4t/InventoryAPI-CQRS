using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Dtos.MovimientoStockDtos;
using MediatR;
using InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;
using InventoryAPI.Features.MovimientosStock.Queries.GetMovimientoById;
using InventoryAPI.Features.MovimientosStock.Commands.CreateMovimiento;
using FluentValidation;

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
    public async Task<IActionResult> GetAll(
    [FromQuery] int page = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetAllMovimientosQuery(page, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetMovimientoById(int id)
    {
        try
        {
            var query = new GetMovimientoByIdQuery(id);

            var result = await _mediator.Send(query);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateMovimientoStockDto dto)
    {
        try
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
        catch (ValidationException ex)
        {
            var errors = ex.Errors
                .GroupBy(e => e.PropertyName)
                .ToDictionary(
                    g => g.Key,
                    g => g.Select(e => e.ErrorMessage).ToArray()
                );

            return BadRequest(new { errors });
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }
    }

}