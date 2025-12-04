using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Repositories;
using MediatR;

namespace InventoryAPI.Features.Stats.ProveedorStats.Queries.GetProveedoresMasActivos;

public class GetProveedoresMasActivosQueryHandler : IRequestHandler<GetProveedoresMasActivosQuery, ProveedorResponseDto?>
{
    private readonly IProveedorRepository _proveedorRepository;
    public GetProveedoresMasActivosQueryHandler(IProveedorRepository proveedorRepository)
    {
        _proveedorRepository = proveedorRepository;
    }

    public async Task<ProveedorResponseDto?> Handle(GetProveedoresMasActivosQuery request, CancellationToken cancellationToken)
    {
        return await _proveedorRepository.GetProveedorMasActivoAsync();
    }
}