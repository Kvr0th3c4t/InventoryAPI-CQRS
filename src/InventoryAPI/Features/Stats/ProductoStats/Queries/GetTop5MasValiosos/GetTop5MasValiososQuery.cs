using InventoryAPI.Dtos.ProductoDtos;
using MediatR;

namespace InventoryAPI.Features.Stats.ProductoStats.Queries.GetTop5MasValiosos;

public record GetTop5MasValiososQuery() : IRequest<IEnumerable<ProductoResponseDto>>;