using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioMasBajo;

public class GetPrecioMasBajoQueryHandler : IRequestHandler<GetPrecioMasBajoQuery, decimal>
{
    private readonly IProductoRepository _productoRepository;

    public GetPrecioMasBajoQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<decimal> Handle(GetPrecioMasBajoQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetPrecioMasBajoAsync();
    }

}
