using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetProductosUltimos30Dias;

public class GetProductosUltimos30DiasQueryHandler : IRequestHandler<GetProductosUltimos30DiasQuery, int>
{
    private readonly IProductoRepository _productoRepository;

    public GetProductosUltimos30DiasQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<int> Handle(GetProductosUltimos30DiasQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetProductosUltimos30DiasAsync();
    }

}
