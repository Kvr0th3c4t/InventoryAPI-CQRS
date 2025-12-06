using InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosPorProveedor;
using InventoryAPI.Features.Stats.ProveedorStats.Queries.GetProveedoresMasActivos;
using InventoryAPI.Features.Stats.ProveedorStats.Queries.GetTotalProveedores;
using InventoryAPI.Features.Stats.ProveedorStats.Queries.GetValorInventarioPorProveedor;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

[ApiController]
[Route("/api/[controller]")]

public class ProveedorStatController : ControllerBase
{
    private readonly IMediator _mediator;

    public ProveedorStatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("total-proveedores")]

    public async Task<IActionResult> GetTotalProveedores()
    {
        var query = new GetTotalProveedoresQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }


    [HttpGet("productos-por-proveedor")]
    public async Task<IActionResult> GetProductosPorProveedor(
        [FromQuery] int pageNumber = 1,
        [FromQuery] int pageSize = 10
    )
    {
        var query = new GetProductosPorProveedorQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("proveedor-mas-activo")]
    public async Task<IActionResult> GetProveedoresMasActivos()
    {
        var query = new GetProveedoresMasActivosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("valor-inventario-por-proveedor")]
    public async Task<IActionResult> GetValorInventarioPorProveedor(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10
    )
    {
        var query = new GetValorInventarioPorProveedorQuery(pageNumber, pageSize);
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}

