using InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioMasAlto;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioMasBajo;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioPromedio;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorCategoria;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorProveedor;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosSinStock;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosStockBajo;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosUltimos30Dias;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MasStock;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MasValiosos;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MenosStock;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetTotalProductos;
using InventoryAPI.Features.Stats.ProductoStats.Queries.GetValorTotalInventario;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class ProductoStatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProductoStatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("total-productos")]
    public async Task<IActionResult> GetTotalProductos()
    {
        var query = new GetTotalProductosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("productos-stock-bajo")]

    public async Task<IActionResult> GetProductosStockBajo()
    {
        var query = new GetProductosStockBajoQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("productos-sin-stock")]

    public async Task<IActionResult> GetProductosSinStock()
    {
        var query = new GetProductosSinStockQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("valor-total-inventario")]

    public async Task<IActionResult> GetValorTotalInventario()
    {
        var query = new GetValorTotalInventarioQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("precio-promedio")]

    public async Task<IActionResult> GetPrecioPromedio()
    {
        var query = new GetPrecioPromedioQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("precio-mas-alto")]

    public async Task<IActionResult> GetPrecioMasAlto()
    {
        var query = new GetPrecioMasAltoQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("precio-mas-bajo")]

    public async Task<IActionResult> GetPrecioMasBajo()
    {
        var query = new GetPrecioMasBajoQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("productos-ultimos-30-dias")]

    public async Task<IActionResult> GetProductosUltimos30Dias()
    {
        var query = new GetProductosUltimos30DiasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("distribucion-por-categoria")]
    public async Task<IActionResult> GetProductosPorCategoria(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetProductosPorCategoriaQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("distribucion-por-proveedor")]

    public async Task<IActionResult> GetProductosPorProveedor(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10
    )
    {
        var query = new GetProductosPorProveedorQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("top5-productos-mas-valiosos")]

    public async Task<IActionResult> GetTop5MasValiosos()
    {
        var query = new GetTop5MasValiososQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("top5-productos-mas-stock")]

    public async Task<IActionResult> GetTop5MasStock()
    {
        var query = new GetTop5MasStockQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("top5-productos-menos-stock")]

    public async Task<IActionResult> GetTop5MenosStock()
    {
        var query = new GetTop5MenosStockQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

}