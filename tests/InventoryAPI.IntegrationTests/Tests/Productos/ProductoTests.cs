using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.Categorias;

public class ProductoTests : IntegrationTestsBase
{
    [Fact]
    public async Task Create_Producto_ReturnCreatedProducto()
    {
        //Given

        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var Id = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portatil",
            CategoriaId = Id,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        };

        //When
        var response = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);

        //Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var producto = await response.Content.ReadFromJsonAsync<ProductoResponseDto>();
        producto.Should().NotBeNull();
        producto!.Id.Should().BeGreaterThan(0);
        producto.Nombre.Should().Be("Portatil");
        producto.Descripcion.Should().BeNull();
        producto.SKU.Should().NotBeNull();
        producto.CategoriaId.Should().Be(1);
        producto.CategoriaNombre.Should().Be("Electrónica");
        producto.StockActual.Should().Be(100);
        producto.Precio.Should().Be(12);
    }

    [Fact]
    public async Task GetAll_Productos_ReturnProductoList()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var Id = createdCategoria!.Id;

        await Client.PostAsJsonAsync("/api/Productos", new CreateProductoDto
        {
            Nombre = "Portatil",
            CategoriaId = Id,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        });

        await Client.PostAsJsonAsync("/api/Productos", new CreateProductoDto
        {
            Nombre = "Tablet",
            CategoriaId = Id,
            StockActual = 800,
            StockMinimo = 1200,
            Precio = 24
        });

        // When
        var response = await Client.GetAsync("/api/Productos");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var productos = await response.Content.ReadFromJsonAsync<List<ProductoResponseDto>>();
        productos.Should().NotBeNull();
        productos.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_Producto_ReturnProducto()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var IdCategoria = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portatil",
            CategoriaId = IdCategoria,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var IdProducto = createdProducto!.Id;

        // When
        var response = await Client.GetAsync($"/api/Productos/{IdProducto}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var producto = await response.Content.ReadFromJsonAsync<ProductoResponseDto>();
        producto.Should().NotBeNull();
        producto!.Id.Should().Be(IdProducto);
        producto.Nombre.Should().Be("Portatil");
        producto.Descripcion.Should().BeNull();
        producto.SKU.Should().NotBeNull();
        producto.CategoriaId.Should().Be(IdCategoria);
        producto.CategoriaNombre.Should().Be("Electrónica");
        producto.StockActual.Should().Be(100);
        producto.Precio.Should().Be(12);

    }

    [Fact]
    public async Task Update_Producto_ReturnUpdatedProducto()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoria2Dto = new CreateCategoriaDto
        {
            Nombre = "Categoria 2",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var IdCategoria = createdCategoria!.Id;

        var createCategoriaResponse2 = await Client.PostAsJsonAsync("/api/Categoria", createCategoria2Dto);
        var createdCategoria2 = await createCategoriaResponse2.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var IdCategoria2 = createdCategoria2!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portatil",
            CategoriaId = IdCategoria,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var IdProducto = createdProducto!.Id;

        var updateDto = new UpdateProductoDto
        {
            Nombre = "Producto modificado",
            SKU = "SKU modificado",
            CategoriaId = IdCategoria2,
            Descripcion = "Descripcion modificada",
            StockActual = 50,
            StockMinimo = 100,
            Precio = 10
        };

        // When
        var response = await Client.PutAsJsonAsync($"/api/Productos/{IdProducto}", updateDto);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var producto = await response.Content.ReadFromJsonAsync<ProductoResponseDto>();
        producto.Should().NotBeNull();
        producto!.Id.Should().Be(IdProducto);
        producto.Nombre.Should().Be("Producto modificado");
        producto.Descripcion.Should().Be("Descripcion modificada");
        producto.SKU.Should().NotBeNull();
        producto.CategoriaId.Should().Be(IdCategoria2);
        producto.CategoriaNombre.Should().Be("Categoria 2");
        producto.StockActual.Should().Be(50);
        producto.Precio.Should().Be(10);

    }

    [Fact]
    public async Task Delete_Producto_ReturnNoContent()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var IdCategoria = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portatil",
            CategoriaId = IdCategoria,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var IdProducto = createdProducto!.Id;


        // When
        var response = await Client.DeleteAsync($"/api/Productos/{IdProducto}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkDelete = await Client.GetAsync($"/api/Productos/{IdProducto}");

        checkDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }
}