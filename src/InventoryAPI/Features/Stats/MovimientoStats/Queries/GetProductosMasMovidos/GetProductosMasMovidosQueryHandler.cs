using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductosMasMovidos;

public class GetProductosMasMovidosQueryHandler : IRequestHandler<GetProductosMasMovidosQuery, IEnumerable<ProductoMasMovidoDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetProductosMasMovidosQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<IEnumerable<ProductoMasMovidoDto>> Handle(GetProductosMasMovidosQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetProductosMasMovidosAsync();
    }
}