using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.Productos;

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
            ProveedorId = null,
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
        producto.Nombre.Should().Be(createProductoDto.Nombre);
        producto.Descripcion.Should().BeNull();
        producto.SKU.Should().NotBeNull();
        producto.CategoriaId.Should().Be(1);
        producto.CategoriaNombre.Should().Be(createCategoriaDto.Nombre);
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
            ProveedorId = null,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        });

        await Client.PostAsJsonAsync("/api/Productos", new CreateProductoDto
        {
            Nombre = "Tablet",
            CategoriaId = Id,
            ProveedorId = null,
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
            ProveedorId = null,
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
        producto.Nombre.Should().Be(createProductoDto.Nombre);
        producto.Descripcion.Should().BeNull();
        producto.SKU.Should().NotBeNull();
        producto.CategoriaId.Should().Be(IdCategoria);
        producto.CategoriaNombre.Should().Be(createCategoriaDto.Nombre);
        producto.StockActual.Should().Be(100);
        producto.Precio.Should().Be(12);

    }
    [Fact]
    public async Task Update_Producto_ReturnUpdatedProducto()
    {
        // Crear categoría original
        var categoriaOriginal = new CreateCategoriaDto { Nombre = "Electrónica", Descripcion = "Productos electrónicos" };
        var respCat1 = await Client.PostAsJsonAsync("/api/Categoria", categoriaOriginal);
        respCat1.EnsureSuccessStatusCode();
        var cat1 = await respCat1.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        Console.WriteLine($"Categoria1 Id={cat1!.Id}");

        // Crear categoría de actualización
        var categoriaUpdate = new CreateCategoriaDto { Nombre = "Categoria 2", Descripcion = "Productos electrónicos" };
        var respCat2 = await Client.PostAsJsonAsync("/api/Categoria", categoriaUpdate);
        respCat2.EnsureSuccessStatusCode();
        var cat2 = await respCat2.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        Console.WriteLine($"Categoria2 Id={cat2!.Id}");
        Console.WriteLine($"CategoriaUpdate creada: Id={cat2.Id}, Nombre={cat2.Nombre}");

        // Crear producto
        var productoDto = new CreateProductoDto
        {
            Nombre = "Portatil",
            CategoriaId = cat1.Id,
            StockActual = 100,
            StockMinimo = 200,
            Precio = 12
        };

        var respProd = await Client.PostAsJsonAsync("/api/Productos", productoDto);
        respProd.EnsureSuccessStatusCode();
        var producto = await respProd.Content.ReadFromJsonAsync<ProductoResponseDto>();
        Console.WriteLine($"Producto Id={producto!.Id}");

        // DTO para update
        var updateDto = new UpdateProductoDto
        {
            Nombre = "Producto modificado",
            SKU = "SKU modificado",
            CategoriaId = cat2.Id,
            Descripcion = "Descripcion modificada",
            StockActual = 50,
            StockMinimo = 100,
            Precio = 10m
        };

        // Ejecutar PUT
        var respUpdate = await Client.PutAsJsonAsync($"/api/Productos/{producto.Id}", updateDto);
        var body = await respUpdate.Content.ReadAsStringAsync();
        Console.WriteLine($"PUT /api/Productos/{producto.Id} response: {(int)respUpdate.StatusCode} - {body}");

        respUpdate.StatusCode.Should().Be(HttpStatusCode.OK);

        var updatedProducto = await respUpdate.Content.ReadFromJsonAsync<ProductoResponseDto>();
        updatedProducto!.CategoriaId.Should().Be(cat2.Id);
        updatedProducto.Nombre.Should().Be(updateDto.Nombre);
        updatedProducto.Descripcion.Should().Be(updateDto.Descripcion);
        updatedProducto.StockActual.Should().Be(50);
        updatedProducto.Precio.Should().Be(10m);
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
            ProveedorId = null,
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