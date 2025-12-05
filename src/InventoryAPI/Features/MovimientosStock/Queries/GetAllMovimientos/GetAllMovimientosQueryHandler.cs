using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Repositories;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;

public class GetAllMovimientosQueryHandler : IRequestHandler<GetAllMovimientosQuery, PagedResponse<MovimientoStockResponseDto>>
{
    private readonly IMovimientoStockRepository _movimientosRepository;
    private readonly IProductoRepository _productoRepository;
    private readonly IProveedorRepository _proveedorRepository;

    public GetAllMovimientosQueryHandler(
        IMovimientoStockRepository movimientosRepository,
        IProductoRepository productoRepository,
        IProveedorRepository proveedorRepository)
    {
        _movimientosRepository = movimientosRepository;
        _productoRepository = productoRepository;
        _proveedorRepository = proveedorRepository;
    }

    public async Task<PagedResponse<MovimientoStockResponseDto>> Handle(GetAllMovimientosQuery request, CancellationToken cancellationToken)
    {
        var page = request.Page < 1 ? 1 : request.Page;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _movimientosRepository.GetAllPaginated(page, pageSize);
    }
}