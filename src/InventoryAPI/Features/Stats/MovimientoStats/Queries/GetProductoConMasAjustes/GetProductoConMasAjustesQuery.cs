using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.MovimientoStats.Queries.GetProductoConMasAjustes;

public record GetProductoConMasAjustesQuery : IRequest<ProductoResponseDto?>;