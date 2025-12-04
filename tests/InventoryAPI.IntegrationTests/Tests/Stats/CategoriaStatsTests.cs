using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.Stats;

public class CategoriaStatsTests : IntegrationTestsBase
{
    private async Task SeedTestDataAsync()
    {
        // Crear categorías
        var categorias = new List<CreateCategoriaDto>
        {
            new CreateCategoriaDto { Nombre = "Electrónica", Descripcion = "Productos electrónicos" },
            new CreateCategoriaDto { Nombre = "Ropa", Descripcion = "Prendas de vestir" },
            new CreateCategoriaDto { Nombre = "Alimentos", Descripcion = "Comida y bebidas" }
        };

        var categoriasCreadas = new List<CategoriaResponseDto>();
        foreach (var categoria in categorias)
        {
            var response = await Client.PostAsJsonAsync("/api/Categoria", categoria);
            var created = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();
            categoriasCreadas.Add(created!);
        }

        // Crear proveedor
        var proveedor = new CreateProveedorDto
        {
            Nombre = "Proveedor Test",
            Email = "test@test.com",
            Telefono = "123456789"
        };
        var proveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedor);
        var proveedorCreated = await proveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();

        // Crear productos distribuidos en categorías
        var productos = new List<CreateProductoDto>
        {
            // 3 productos en Electrónica (mayor valor)
            new CreateProductoDto { Nombre = "Laptop", CategoriaId = categoriasCreadas[0].Id, ProveedorId = proveedorCreated!.Id,  StockActual = 50, StockMinimo = 10, Precio = 1000 },
            new CreateProductoDto { Nombre = "Mouse", CategoriaId = categoriasCreadas[0].Id, ProveedorId = proveedorCreated!.Id,  StockActual = 100, StockMinimo = 20, Precio = 50 },
            new CreateProductoDto { Nombre = "Teclado", CategoriaId = categoriasCreadas[0].Id, ProveedorId = proveedorCreated!.Id,  StockActual = 80, StockMinimo = 15, Precio = 80 },
            
            // 2 productos en Ropa
            new CreateProductoDto { Nombre = "Camisa", CategoriaId = categoriasCreadas[1].Id,ProveedorId = proveedorCreated!.Id, StockActual = 30, StockMinimo = 5, Precio = 40 },
            new CreateProductoDto { Nombre = "Pantalón", CategoriaId = categoriasCreadas[1].Id,ProveedorId = proveedorCreated!.Id, StockActual = 25, StockMinimo = 5, Precio = 60 },
            
            // 1 producto en Alimentos
            new CreateProductoDto { Nombre = "Arroz", CategoriaId = categoriasCreadas[2].Id, ProveedorId = proveedorCreated!.Id, StockActual = 200, StockMinimo = 50, Precio = 5 }
        };

        foreach (var producto in productos)
        {
            await Client.PostAsJsonAsync("/api/Productos", producto);
        }
    }

    [Fact]
    public async Task GetTotalCategorias_DebeRetornarCantidadCorrecta()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/CategoriaStat/total-categorias");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.Content.ReadFromJsonAsync<int>();
        total.Should().Be(3);
    }

    [Fact]
    public async Task GetDistribucionProductosPorCategoria_DebeRetornarAgrupacionCorrecta()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/CategoriaStat/distribucion-productos-por-categoria");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var distribucion = await response.Content.ReadFromJsonAsync<List<DistribucionCategoriaDto>>();

        distribucion.Should().NotBeNull();
        distribucion.Should().HaveCount(3);

        var electronica = distribucion!.First(d => d.NombreCategoria == "Electrónica");
        electronica.CantidadProductos.Should().Be(3);

        var ropa = distribucion!.First(d => d.NombreCategoria == "Ropa");
        ropa.CantidadProductos.Should().Be(2);

        var alimentos = distribucion.First(d => d.NombreCategoria == "Alimentos");
        alimentos.CantidadProductos.Should().Be(1);
    }

    [Fact]
    public async Task GetCategoriaConMasProductos_DebeRetornarElectrónica()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/CategoriaStat/categoria-con-mas-productos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoria = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();

        categoria.Should().NotBeNull();
        categoria!.Nombre.Should().Be("Electrónica");
    }

    [Fact]
    public async Task GetCategoriaConMayorValor_DebeRetornarElectrónica()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/CategoriaStat/categoria-con-mayor-valor");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var categoria = await response.Content.ReadFromJsonAsync<CategoriaResponseDto>();

        categoria.Should().NotBeNull();
        // Electrónica: (50*1000) + (100*50) + (80*80) = 50000 + 5000 + 6400 = 61400
        // Es la de mayor valor
        categoria!.Nombre.Should().Be("Electrónica");
    }
}