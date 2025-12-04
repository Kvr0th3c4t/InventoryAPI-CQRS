using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosStockBajo;

public class GetProductosStockBajoQueryHandler : IRequestHandler<GetProductosStockBajoQuery, int>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosStockBajoQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<int> Handle(GetProductosStockBajoQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetProductosStockBajoAsync();
    }

}
