
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetEntradasVsSalidasUltimos30Dias;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorDia;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetMovimientosPorProveedor;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductoConMasAjustes;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductosMasMovidos;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTipoMovimientos;
using InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTotalMovimientosUltimos30Dias;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

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
    public async Task<IActionResult> GetMovimientosPorDia()
    {
        var query = new GetMovimientosPorDiaQuery();
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
    public async Task<IActionResult> GetTipoMovimientos()
    {
        var query = new GetTipoMovimientosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("movimientos-por-proveedor")]
    public async Task<IActionResult> GetMovimientosPorProveedor()
    {
        var query = new GetMovimientosPorProveedorQuery();
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
