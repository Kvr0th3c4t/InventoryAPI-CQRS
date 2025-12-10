using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Features.Categorias.Commands.CreateCategoria;
using InventoryAPI.Features.Categorias.Commands.DeleteCategoria;
using InventoryAPI.Features.Categorias.Commands.UpdateCategoria;
using InventoryAPI.Features.Categorias.Queries.GetAllCategorias;
using InventoryAPI.Features.Categorias.Queries.GetCategoriaById;
using InventoryAPI.Dtos.Pagination;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.AspNetCore.Authorization;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]

public class CategoriaController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriaController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetAllCategoriasQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCategoriaById(int id)
    {
        try
        {
            var query = new GetCategoriaByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }

    }

    [HttpPost]
    public async Task<ActionResult> Create([FromBody] CreateCategoriaDto dto)
    {
        var command = new CreateCategoriaCommand
        (
            dto.Nombre,
            dto.Descripcion
        );

        var result = await _mediator.Send(command);
        return CreatedAtAction(nameof(GetCategoriaById), new { id = result.Id }, result);
    }

    [HttpPut("{id}")]
    public async Task<ActionResult> UpdateCategoria(int id, [FromBody] UpdateCategoriaDto dto)
    {
        try
        {
            var command = new UpdateCategoriaCommand
        (
            id,
            dto.Nombre,
            dto.Descripcion
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
    public async Task<ActionResult> DeleteCategoria(int id)
    {
        try
        {
            var command = new DeleteCategoriaCommand(id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }


}