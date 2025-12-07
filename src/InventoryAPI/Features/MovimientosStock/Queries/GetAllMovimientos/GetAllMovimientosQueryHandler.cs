using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Repositories;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.MovimientosStock.Queries.GetAllMovimientos;

public class GetAllMovimientosStockQueryHandler
    : IRequestHandler<GetAllMovimientosQuery, PagedResponse<MovimientoStockResponseDto>>
{
    private readonly IMovimientoStockRepository _movimientoStockRepository;

    public GetAllMovimientosStockQueryHandler(IMovimientoStockRepository movimientoStockRepository)
    {
        _movimientoStockRepository = movimientoStockRepository;
    }

    public async Task<PagedResponse<MovimientoStockResponseDto>> Handle(
        GetAllMovimientosQuery request,
        CancellationToken cancellationToken)
    {
        // Validar paginación
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        // Validar ordenamiento
        var validOrderByFields = new[] { "fecha", "cantidad", "tipo", "producto" };
        var orderBy = !string.IsNullOrWhiteSpace(request.OrderBy) &&
                      validOrderByFields.Contains(request.OrderBy.ToLower())
            ? request.OrderBy.ToLower()
            : "fecha";

        var order = request.Order?.ToLower() == "asc" ? "asc" : "desc";

        // Llamar al repository con todos los filtros
        return await _movimientoStockRepository.GetAllPaginated(
            fechaDesde: request.FechaDesde,
            fechaHasta: request.FechaHasta,
            tipo: request.Tipo,
            productoId: request.ProductoId,
            orderBy: orderBy,
            order: order,
            pageNumber: pageNumber,
            pageSize: pageSize
        );
    }
}