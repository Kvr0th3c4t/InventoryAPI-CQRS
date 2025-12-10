using Azure;
using InventoryAPI.Features.Stats.CategoriaStats.Queries.GetCategoriaConMasProductos;
using InventoryAPI.Features.Stats.CategoriaStats.Queries.GetCategoriaConMayorValor;
using InventoryAPI.Features.Stats.CategoriaStats.Queries.GetDistribucionProductosPorCategoria;
using InventoryAPI.Features.Stats.CategoriaStats.Queries.GetTotalCategorias;

using MediatR;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace InventoryAPI.Controllers;

[Authorize]
[ApiController]
[Route("/api/[controller]")]
public class CategoriaStatController : ControllerBase
{
    private readonly IMediator _mediator;

    public CategoriaStatController(IMediator mediator)
    {
        _mediator = mediator;
    }

    [HttpGet("total-categorias")]

    public async Task<IActionResult> GetTotalCategorias()
    {
        var query = new GetTotalCategoriasQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("distribucion-productos-por-categoria")]
    public async Task<IActionResult> GetDistribucionProductosPorCategoria(
    [FromQuery] int pageNumber = 1,
    [FromQuery] int pageSize = 10)
    {
        var query = new GetDistribucionProductosPorCategoriaQuery
        {
            PageNumber = pageNumber,
            PageSize = pageSize
        };

        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("categoria-con-mas-productos")]

    public async Task<IActionResult> GetCategoriaConMasProductos()
    {
        var query = new GetCategoriaConMasProductosQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }

    [HttpGet("categoria-con-mayor-valor")]

    public async Task<IActionResult> GetCategoriasConMayorValor()
    {
        var query = new GetCategoriaConMayorValorQuery();
        var result = await _mediator.Send(query);
        return Ok(result);
    }
}
