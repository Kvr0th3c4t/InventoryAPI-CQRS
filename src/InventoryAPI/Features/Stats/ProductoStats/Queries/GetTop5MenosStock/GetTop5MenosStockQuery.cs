using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MenosStock;

public record GetTop5MenosStockQuery() : IRequest<IEnumerable<ProductoResponseDto>>;