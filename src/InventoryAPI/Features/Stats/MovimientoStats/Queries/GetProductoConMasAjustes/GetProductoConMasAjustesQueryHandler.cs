using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.StatsDtos.MovimientosStatsDto;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductoConMasAjustes;

public class GetProductoConMasAjustesQueryHandler : IRequestHandler<GetProductoConMasAjustesQuery, ProductoResponseDto?>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;
    public GetProductoConMasAjustesQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<ProductoResponseDto?> Handle(GetProductoConMasAjustesQuery request, CancellationToken cancellationToken)
    {
        return await _movimientoStockRepository.GetProductoConMasAjustesAsync();
    }
}