using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MenosStock;

public class GetTop5MenosStockQueryHandler : IRequestHandler<GetTop5MenosStockQuery, IEnumerable<ProductoResponseDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetTop5MenosStockQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<ProductoResponseDto>> Handle(GetTop5MenosStockQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetTop5MenosStockAsync();
    }

}
