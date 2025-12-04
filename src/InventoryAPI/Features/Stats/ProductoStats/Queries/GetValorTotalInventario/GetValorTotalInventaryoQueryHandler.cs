using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetValorTotalInventario;

public class GetValorTotalInventarioQueryHandler : IRequestHandler<GetValorTotalInventarioQuery, decimal>
{
    private readonly IProductoRepository _productoRepository;

    public GetValorTotalInventarioQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<decimal> Handle(GetValorTotalInventarioQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetValorTotalInventarioAsync();
    }

}
