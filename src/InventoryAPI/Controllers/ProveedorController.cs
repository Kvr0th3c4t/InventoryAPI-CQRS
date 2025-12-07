using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Features.Proveedores.Commands.CreateProveedor;
using InventoryAPI.Features.Proveedores.Commands.DeleteProveedor;
using InventoryAPI.Features.Proveedores.Commands.UpdateProveedor;
using InventoryAPI.Features.Proveedores.Queries.GetAllProveedores;
using InventoryAPI.Features.Proveedores.Queries.GetProveedorById;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Dtos.Pagination;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("api/[controller]")]
public class ProveedorController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProveedorController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetAllProveedoresQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetProveedorById(int id)
    {
        try
        {
            var query = new GetProveedorByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateProveedorDto dto)
    {
        var command = new CreateProveedorCommand
        (
            dto.Nombre,
            dto.Email,
            dto.Telefono
        );

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetProveedorById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateProveedor(int id, [FromBody] UpdateProveedorDto dto)
    {
        try
        {
            var command = new UpdateProveedorCommand(
            id,
            dto.Nombre,
            dto.Email,
            dto.Telefono
        );

            var result = await _mediator.Send(command);

            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }

    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteProveedor(int id)
    {
        try
        {
            var command = new DeleteProveedorCommand(id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
        catch (InvalidOperationException ex)
        {
            return BadRequest(ex.Message);
        }

    }
}