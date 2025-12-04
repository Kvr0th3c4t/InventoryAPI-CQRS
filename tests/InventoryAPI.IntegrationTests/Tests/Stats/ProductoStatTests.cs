using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.Stats;

public class ProductoStatsTests : IntegrationTestsBase
{
    private async Task<int> SeedTestDataAsync()
    {
        // Crear categoría
        var categoria = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };
        var categoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", categoria);
        var categoriaCreated = await categoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();

        // Crear proveedor
        var proveedor = new CreateProveedorDto
        {
            Nombre = "Proveedor Test",
            Email = "test@test.com",
            Telefono = "123456789"
        };
        var proveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedor);
        var proveedorCreated = await proveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();

        // Crear productos con diferentes características
        var productos = new List<CreateProductoDto>
        {
            new CreateProductoDto
            {
                Nombre = "Producto Alto Stock",
                CategoriaId = categoriaCreated!.Id,
ProveedorId = proveedorCreated!.Id,
                StockActual = 100,
                StockMinimo = 10,
                Precio = 1000
            },
            new CreateProductoDto
            {
                Nombre = "Producto Stock Bajo",
                CategoriaId = categoriaCreated!.Id,
ProveedorId = proveedorCreated!.Id,
                StockActual = 5,
                StockMinimo = 10,
                Precio = 50
            },
            new CreateProductoDto
            {
                Nombre = "Producto Sin Stock",
                CategoriaId = categoriaCreated!.Id,
ProveedorId = proveedorCreated!.Id,
                StockActual = 0,
                StockMinimo = 10,
                Precio = 30
            },
            new CreateProductoDto
            {
                Nombre = "Producto Normal",
                CategoriaId = categoriaCreated!.Id,
ProveedorId = proveedorCreated!.Id,
                StockActual = 50,
                StockMinimo = 20,
                Precio = 100
            }
        };

        foreach (var producto in productos)
        {
            await Client.PostAsJsonAsync("/api/Productos", producto);
        }

        return productos.Count;
    }

    [Fact]
    public async Task GetTotalProductos_DebeRetornarCantidadCorrecta()
    {
        // Arrange
        var expectedCount = await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/total-productos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.Content.ReadFromJsonAsync<int>();
        total.Should().Be(expectedCount);
    }

    [Fact]
    public async Task GetProductosStockBajo_DebeRetornarCantidadCorrecta()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/productos-stock-bajo");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.Content.ReadFromJsonAsync<int>();
        total.Should().Be(2); // Producto Stock Bajo + Producto Sin Stock
    }

    [Fact]
    public async Task GetProductosSinStock_DebeRetornarCantidadCorrecta()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/productos-sin-stock");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.Content.ReadFromJsonAsync<int>();
        total.Should().Be(1); // Solo Producto Sin Stock
    }

    [Fact]
    public async Task GetValorTotalInventario_DebeCalcularCorrectamente()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/valor-total-inventario");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var valorTotal = await response.Content.ReadFromJsonAsync<decimal>();

        // (100 * 1000) + (5 * 50) + (0 * 30) + (50 * 100) = 100000 + 250 + 0 + 5000 = 105250
        valorTotal.Should().Be(105250);
    }

    [Fact]
    public async Task GetPrecioPromedio_DebeCalcularCorrectamente()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/precio-promedio");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var precioPromedio = await response.Content.ReadFromJsonAsync<decimal>();

        // (1000 + 50 + 30 + 100) / 4 = 295
        precioPromedio.Should().Be(295);
    }

    [Fact]
    public async Task GetPrecioMasAlto_DebeRetornarMaximo()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/precio-mas-alto");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var precioMax = await response.Content.ReadFromJsonAsync<decimal>();
        precioMax.Should().Be(1000);
    }

    [Fact]
    public async Task GetPrecioMasBajo_DebeRetornarMinimo()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/precio-mas-bajo");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var precioMin = await response.Content.ReadFromJsonAsync<decimal>();
        precioMin.Should().Be(30);
    }

    [Fact]
    public async Task GetProductosUltimos30Dias_DebeContarProductosRecientes()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/productos-ultimos-30-dias");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.Content.ReadFromJsonAsync<int>();
        total.Should().Be(4); 
    }

    [Fact]
    public async Task GetDistribucionPorCategoria_DebeRetornarAgrupacion()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/distribucion-por-categoria");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var distribucion = await response.Content.ReadFromJsonAsync<List<DistribucionCategoriaDto>>();

        distribucion.Should().NotBeNull();
        distribucion.Should().HaveCount(1);
        distribucion![0].NombreCategoria.Should().Be("Electrónica");
        distribucion[0].CantidadProductos.Should().Be(4);
    }

    [Fact]
    public async Task GetDistribucionPorProveedor_DebeRetornarAgrupacion()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/distribucion-por-proveedor");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var distribucion = await response.Content.ReadFromJsonAsync<List<DistribucionProveedorDto>>();

        distribucion.Should().NotBeNull();
        distribucion.Should().HaveCount(1); // Solo un proveedor
        distribucion![0].NombreProveedor.Should().Be("Proveedor Test");
        distribucion[0].CantidadProductos.Should().Be(4);
    }

    [Fact]
    public async Task GetTop5ProductosMasValiosos_DebeRetornarOrdenados()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/top5-productos-mas-valiosos");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productos = await response.Content.ReadFromJsonAsync<List<ProductoResponseDto>>();

        productos.Should().NotBeNull();
        productos.Should().HaveCountLessThanOrEqualTo(5);

        // El primer producto debe ser el de mayor valor (stock * precio)
        productos![0].Nombre.Should().Be("Producto Alto Stock"); // 100 * 1000 = 100000
    }

    [Fact]
    public async Task GetTop5ProductosMasStock_DebeRetornarOrdenados()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/top5-productos-mas-stock");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productos = await response.Content.ReadFromJsonAsync<List<ProductoResponseDto>>();

        productos.Should().NotBeNull();
        productos.Should().HaveCountLessThanOrEqualTo(5);
        productos![0].StockActual.Should().Be(100);
    }

    [Fact]
    public async Task GetTop5ProductosMenosStock_DebeRetornarOrdenados()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProductoStat/top5-productos-menos-stock");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var productos = await response.Content.ReadFromJsonAsync<List<ProductoResponseDto>>();

        productos.Should().NotBeNull();
        productos.Should().HaveCountLessThanOrEqualTo(5);
        productos![0].StockActual.Should().Be(5);
    }
}