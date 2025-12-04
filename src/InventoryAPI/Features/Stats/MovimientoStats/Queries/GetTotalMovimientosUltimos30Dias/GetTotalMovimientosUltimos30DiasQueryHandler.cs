using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetTotalMovimientosUltimos30Dias;

public class GetTotalMovimientosUltimos30DiasQueryHandler : IRequestHandler<GetTotalMovimientosUltimos30DiasQuery, int>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetTotalMovimientosUltimos30DiasQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<int> Handle(GetTotalMovimientosUltimos30DiasQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetTotalMovimientosUltimos30DiasAsync();
    }
}