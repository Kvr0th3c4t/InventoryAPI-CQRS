
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetEntradasVsSalidasUltimos30Dias;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorDia;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorProveedor;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductoConMasAjustes;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductosMasMovidos;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTipoMovimientos;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTotalMovimientosUltimos30Dias;
using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]

public class MovimientoStockStatController : ControllerBase
{
    private readonly IMediator _mediator;

    public MovimientoStockStatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("total-movimientos-ultimos-30-dias")]
    public async Task<IActionResult> GetTotalMovimientosUltimos30Dias()
    {
        var query = new GetTotalMovimientosUltimos30DiasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("entradas-vs-salidas-ultimos-30-dias")]
    public async Task<IActionResult> GetEntradasVsSalidasUltimos30Dias()
    {
        var query = new GetEntradasVsSalidasUltimos30DiasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("movimientos-por-dia")]
    public async Task<IActionResult> GetMovimientosPorDia(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetMovimientosPorDiaQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("productos-mas-movidos")]
    public async Task<IActionResult> GetProductosMasMovidos()
    {
        var query = new GetProductosMasMovidosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("tipo-movimientos")]
    public async Task<IActionResult> GetTipoMovimientos(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetTipoMovimientosQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("movimientos-por-proveedor")]
    public async Task<IActionResult> GetMovimientosPorProveedor(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetMovimientosPorProveedorQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("producto-con-mas-ajustes")]
    public async Task<IActionResult> GetProductoConMasAjustes()
    {
        var query = new GetProductoConMasAjustesQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
