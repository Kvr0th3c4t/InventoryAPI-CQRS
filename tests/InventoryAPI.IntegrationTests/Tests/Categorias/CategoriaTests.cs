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
        //Given
        var createDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        //When
        var response = await Client.PostAsJsonAsync("/api/Categoria", createDto);

        //Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var categoria = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        categoria.Should().NotBeNull();
        categoria!.Id.Should().BeGreaterThan(0);
        categoria.Nombre.Should().Be("Electrónica");
        categoria.Descripcion.Should().Be("Productos electrónicos");
    }

    [Fact]
    public async Task GetAll_Categoria_ReturnCategoriaList()
    {
        // Given
        await Client.PostAsJsonAsync("/api/Categoria", new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        });

        await Client.PostAsJsonAsync("/api/Categoria", new CreateCategoriaDto
        {
            Nombre = "Moda",
            Descripcion = "Productos de moda"
        });

        // When
        var response = await Client.GetAsync("/api/Categoria");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categorias = await response.Content.ReadFromJsonAsync<List<CategoriaResponseDto>>();
        categorias.Should().NotBeNull();
        categorias.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_Categoria_ReturnCategoria()
    {
        // Given
        var createDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createResponse = await Client.PostAsJsonAsync("/api/Categoria", createDto);
        var createdCategoria = await createResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var Id = createdCategoria!.Id;

        // When
        var response = await Client.GetAsync($"/api/Categoria/{Id}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoria = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();

        categoria.Should().NotBeNull();
        categoria!.Id.Should().Be(Id);
        categoria.Nombre.Should().Be("Electrónica");
        categoria.Descripcion.Should().Be("Productos electrónicos");

    }

    [Fact]
    public async Task Update_Categoria_ReturnUpdatedCategoria()
    {
        // Given
        var createDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createResponse = await Client.PostAsJsonAsync("/api/Categoria", createDto);
        var createdCategoria = await createResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var Id = createdCategoria!.Id;

        var updateDto = new UpdateCategoriaDto
        {
            Nombre = "Nombre modificado",
            Descripcion = "Descripción modificada"
        };

        // When
        var response = await Client.PutAsJsonAsync($"/api/Categoria/{Id}", updateDto);
        // Then

        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var categoria = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();

        categoria!.Id.Should().Be(Id);
        categoria!.Nombre.Should().Be("Nombre modificado");
        categoria!.Descripcion.Should().Be("Descripción modificada");

    }

    [Fact]
    public async Task Delete_Categoria_ReturnNoContent()
    {
        // Given
        var createDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createResponse = await Client.PostAsJsonAsync("/api/Categoria", createDto);
        var createdCategoria = await createResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var Id = createdCategoria!.Id;
        // When
        var response = await Client.DeleteAsync($"/api/Categoria/{Id}");
        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkDelete = await Client.GetAsync($"/api/Categoria/{Id}");

        checkDelete.StatusCode.Should().NotBe(HttpStatusCode.OK);
    }
}