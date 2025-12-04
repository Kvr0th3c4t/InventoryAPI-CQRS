using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTotalProductos;

public class GetTotalProductosQueryHandler : IRequestHandler<GetTotalProductosQuery, int>
{
    private readonly IProductoRepository _productoRepository;

    public GetTotalProductosQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<int> Handle(GetTotalProductosQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetTotalProductosAsync();
    }

}
