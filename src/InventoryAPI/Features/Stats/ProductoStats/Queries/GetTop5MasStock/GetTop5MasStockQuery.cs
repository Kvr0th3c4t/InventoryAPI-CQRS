using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MasStock;

public record GetTop5MasStockQuery() : IRequest<IEnumerable<ProductoResponseDto>>;