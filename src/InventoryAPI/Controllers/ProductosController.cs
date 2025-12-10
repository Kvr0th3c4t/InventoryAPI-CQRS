using Microsoft.AspNetCore.Mvc;
using InventoryAPI.Dtos.ProductoDtos;
using MediatR;
using InventoryAPI.Features.Productos.Commands.CreateProducto;
using InventoryAPI.Features.Productos.Queries.GetProductoById;
using InventoryAPI.Features.Productos.Queries.GetAllProductos;
using InventoryAPI.Features.Productos.DeleteProducto;
using InventoryAPI.Features.Productos.Commands.UpdateProducto;
using Microsoft.AspNetCore.Authorization;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductosController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductosController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet]
    public async Task<IActionResult> GetAll(
     [FromQuery] string? search = null,
     [FromQuery] int? categoriaId = null,
     [FromQuery] int? proveedorId = null,
     [FromQuery] decimal? precioMin = null,
     [FromQuery] decimal? precioMax = null,
     [FromQuery] bool? stockBajo = null,
     [FromQuery] string? orderBy = null,
     [FromQuery] string? order = null,
     [FromQuery] int pageNumber = 1,
     [FromQuery] int pageSize = 10)
    {
        var query = new GetAllProductosQuery(
            Search: search,
            CategoriaId: categoriaId,
            ProveedorId: proveedorId,
            PrecioMin: precioMin,
            PrecioMax: precioMax,
            StockBajo: stockBajo,
            OrderBy: orderBy,
            Order: order,
            PageNumber: pageNumber,
            PageSize: pageSize
        );

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("{id}")]
    public async Task<IActionResult> GetProductById(int id)
    {
        try
        {
            var query = new GetProductoByIdQuery(id);
            var result = await _mediator.Send(query);
            return Ok(result);
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] CreateProductoDto dto)
    {
        try
        {
            var command = new CreateProductoCommand(
                dto.Nombre,
                dto.Descripcion,
                dto.CategoriaId,
                dto.StockActual,
                dto.StockMinimo,
                dto.Precio,
                dto.ProveedorId
            );

            var result = await _mediator.Send(command);
            return CreatedAtAction(nameof(GetProductById), new { id = result.Id }, result);
        }
        catch (ArgumentException ex)
        {
            return BadRequest(ex.Message);
        }
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> UpdateProduct(int id, [FromBody] UpdateProductoDto dto)
    {
        try
        {
            var command = new UpdateProductoCommand(
                id,
                dto.Nombre,
                dto.Descripcion,
                dto.StockActual,
                dto.StockMinimo,
                dto.CategoriaId,
                dto.Precio,
                dto.ProveedorId
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
    public async Task<IActionResult> DeleteProducto(int id)
    {
        try
        {
            var command = new DeleteProductoCommand(id);
            var result = await _mediator.Send(command);
            return result ? NoContent() : NotFound();
        }
        catch (KeyNotFoundException)
        {
            return NotFound();
        }
    }
}