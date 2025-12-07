using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Repositories;
using InventoryAPI.Dtos.Pagination;
using MediatR;

namespace InventoryAPI.Features.Categorias.Queries.GetAllCategorias;

public class GetAllCategoriasQueryHandler
    : IRequestHandler<GetAllCategoriasQuery, PagedResponse<CategoriaResponseDto>>
{
    private readonly ICategoriaRepository _categoriaRepository;

    public GetAllCategoriasQueryHandler(ICategoriaRepository categoriaRepository)
    {
        _categoriaRepository = categoriaRepository;
    }

    public async Task<PagedResponse<CategoriaResponseDto>> Handle(
        GetAllCategoriasQuery request,
        CancellationToken cancellationToken)
    {
        var pageNumber = request.PageNumber < 1 ? 1 : request.PageNumber;
        var pageSize = request.PageSize < 1 ? 10 :
                       request.PageSize > 100 ? 100 : request.PageSize;

        return await _categoriaRepository.GetAllPaginated(pageNumber, pageSize);
    }
}