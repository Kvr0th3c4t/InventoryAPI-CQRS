using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.Categorias;

public class CategoriaTests : IntegrationTestsBase
{
    [Fact]
    public async Task Create_Categoria_ReturnCreatedCategoria()
    {
        // Given
        var createDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        // Act
        var response = await Client.PostAsJsonAsync("/api/Categoria", createDto);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoria = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        categoria.Should().NotBeNull();
        categoria!.Id.Should().BeGreaterThan(0);
        categoria.Nombre.Should().Be("Electrónica");
        categoria.Descripcion.Should().Be("Productos electrónicos");
    }
}