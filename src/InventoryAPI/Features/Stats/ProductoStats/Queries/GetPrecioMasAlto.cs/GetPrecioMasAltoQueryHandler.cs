using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetPrecioMasAlto;

public class GetPrecioMasAltoQueryHandler : IRequestHandler<GetPrecioMasAltoQuery, decimal>
{
    private readonly IProductoRepository _productoRepository;

    public GetPrecioMasAltoQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<decimal> Handle(GetPrecioMasAltoQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetPrecioMasAltoAsync();
    }

}
