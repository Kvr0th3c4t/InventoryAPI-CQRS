using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosSinStock;

public class GetProductosSinStockQueryHandler : IRequestHandler<GetProductosSinStockQuery, int>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosSinStockQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<int> Handle(GetProductosSinStockQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetProductosSinStockAsync();
    }

}
