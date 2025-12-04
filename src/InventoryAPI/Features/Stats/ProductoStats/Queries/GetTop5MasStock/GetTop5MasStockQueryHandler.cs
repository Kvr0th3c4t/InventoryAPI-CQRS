using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MasStock;

public class GetTop5MasStockQueryHandler : IRequestHandler<GetTop5MasStockQuery, IEnumerable<ProductoResponseDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetTop5MasStockQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<ProductoResponseDto>> Handle(GetTop5MasStockQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetTop5MasStockAsync();
    }

}
