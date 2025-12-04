using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioPromedio;

public class GetPrecioPromedioQueryHandler : IRequestHandler<GetPrecioPromedioQuery, decimal>
{
    private readonly IProductoRepository _productoRepository;

    public GetPrecioPromedioQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<decimal> Handle(GetPrecioPromedioQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetPrecioPromedioAsync();
    }

}
