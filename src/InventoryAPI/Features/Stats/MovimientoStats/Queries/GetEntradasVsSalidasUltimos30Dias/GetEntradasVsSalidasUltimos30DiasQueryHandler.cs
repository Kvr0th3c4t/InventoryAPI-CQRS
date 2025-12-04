using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetEntradasVsSalidasUltimos30Dias;

public class GetEntradasVsSalidasUltimos30DiasQueryHandler : IRequestHandler<GetEntradasVsSalidasUltimos30DiasQuery, EntradasVsSalidasDto>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetEntradasVsSalidasUltimos30DiasQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<EntradasVsSalidasDto> Handle(GetEntradasVsSalidasUltimos30DiasQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetEntradasVsSalidasUltimos30DiasAsync();
    }
}