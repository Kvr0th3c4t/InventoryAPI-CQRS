using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Features.Categorias.Commands.CreateCategoria;
using InventoryAPI.Features.Categorias.Commands.DeleteCategoria;
using InventoryAPI.Features.Categorias.Commands.UpdateCategoria;
using InventoryAPI.Features.Categorias.Queries.GetAllCategorias;
using InventoryAPI.Features.Categorias.Queries.GetCategoriaById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

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
    public async Task<ActionResult> GetAll()
    {
        var query = new GetAllCategoriasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<ActionResult> GetCategoriaById(int id)
    {
        var query = new GetCategoriaByIdQuery(id);
        var result = await _mediator.Send(query);
        return Ok(result);
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
        var command = new UpdateCategoriaCommand
        (
            id,
            dto.Nombre,
            dto.Descripcion
        );

        var result = await _mediator.Send(command);
        return Ok(result);
    }

    [HttpDelete("{id}")]
    public async Task<ActionResult> DeleteCategoria(int id)
    {
        var command = new DeleteCategoriaCommand(id);
        await _mediator.Send(command);

        return NoContent();
    }
}