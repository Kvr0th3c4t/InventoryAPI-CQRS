using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.ProductoDtos;
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
        categoria.Nombre.Should().Be(createDto.Nombre);
        categoria.Descripcion.Should().Be(createDto.Descripcion);
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
        categoria.Nombre.Should().Be(createDto.Nombre);
        categoria.Descripcion.Should().Be(createDto.Descripcion);

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
        categoria!.Nombre.Should().Be(updateDto.Nombre);
        categoria!.Descripcion.Should().Be(updateDto.Descripcion);

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

        checkDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Create_Producto_WithInvalidCategoriaId_ReturnsBadRequest()
    {
        // Given 
        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portátil Gaming",
            CategoriaId = 99999,
            ProveedorId = null, // ❌ Este ID no existe
            StockActual = 50,
            StockMinimo = 10,
            Precio = 1299.99m
        };

        // When
        var response = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_Producto_GeneratesUniqueSKU()
    {
        // Given 
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var categoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var categoria = await categoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var categoriaId = categoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Teclado Mecánico",
            CategoriaId = categoriaId,
            ProveedorId = null,
            StockActual = 100,
            StockMinimo = 20,
            Precio = 89.99m
        };

        // When
        var response = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var producto = await response.Content.ReadFromJsonAsync<ProductoResponseDto>();

        // Then 
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        producto!.SKU.Should().NotBeNullOrEmpty();
        producto.SKU.Should().StartWith("PROD-");
        producto.SKU.Length.Should().Be(13);

        var guidPart = producto.SKU.Substring(5);
        guidPart.Should().MatchRegex("^[A-Za-z0-9]{8}$");
    }
}