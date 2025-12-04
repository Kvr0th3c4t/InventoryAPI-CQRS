using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MasValiosos;

public class GetTop5MasValiososQueryHandler : IRequestHandler<GetTop5MasValiososQuery, IEnumerable<ProductoResponseDto>>
{
    private readonly IProductoRepository _productoRepository;

    public GetTop5MasValiososQueryHandler(IProductoRepository productoRepository)
    {
        _productoRepository = productoRepository;
    }

    public async Task<IEnumerable<ProductoResponseDto>> Handle(GetTop5MasValiososQuery request, CancellationToken cancellationToken)
    {
        return await _productoRepository.GetTop5MasValiososAsync();
    }

}
