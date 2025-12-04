using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.Dtos.StatsDtos;
using InventoryAPI.Dtos.StatsDtos.ProveedoresStatsDto;
using InventoryAPI.Dtos.StatsDtos.ProductosStatsDto;
using InventoryAPI.Models;
using InventoryAPI.Enums;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.Stats;

public class ProveedorStatsTests : IntegrationTestsBase
{
    private async Task<List<ProveedorResponseDto>> SeedTestDataAsync()
    {
        var categoria = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };
        var categoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", categoria);
        var categoriaCreated = await categoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();

        var proveedoresDto = new List<CreateProveedorDto>
    {
        new CreateProveedorDto { Nombre = "Proveedor A", Email = "contactoA@test.com", Telefono = "111111111" },
        new CreateProveedorDto { Nombre = "Proveedor B", Email = "contactoB@test.com", Telefono = "222222222" },
        new CreateProveedorDto { Nombre = "Proveedor C", Email = "contactoC@test.com", Telefono = "333333333" }
    };

        var proveedores = new List<ProveedorResponseDto>();
        foreach (var proveedor in proveedoresDto)
        {
            var response = await Client.PostAsJsonAsync("/api/Proveedor", proveedor);
            var created = await response.Content.ReadFromJsonAsync<ProveedorResponseDto>();
            proveedores.Add(created!);
        }

        var productosProveedor = new List<(int proveedorId, string nombre, int stock, decimal precio)>
    {
        (proveedores[0].Id, "Laptop A", 50, 1000),
        (proveedores[0].Id, "Mouse A", 100, 50),
        (proveedores[0].Id, "Teclado A", 80, 80),
        (proveedores[1].Id, "Laptop B", 30, 1200),
        (proveedores[1].Id, "Monitor B", 40, 500),
        (proveedores[2].Id, "Cable C", 200, 10)
    };

        var productosCreados = new List<(int productoId, int proveedorId, string nombre)>();
        foreach (var (proveedorId, nombre, stock, precio) in productosProveedor)
        {
            var producto = new CreateProductoDto
            {
                Nombre = nombre,
                CategoriaId = categoriaCreated!.Id,
                ProveedorId = proveedorId,
                StockActual = stock,
                StockMinimo = 10,
                Precio = precio
            };
            var response = await Client.PostAsJsonAsync("/api/Productos", producto);
            var created = await response.Content.ReadFromJsonAsync<ProductoResponseDto>();

            productosCreados.Add((created!.Id, proveedorId, created.Nombre));
        }

        foreach (var (productoId, proveedorId, nombre) in productosCreados)
        {
            int movimientos = nombre.Contains("A") ? 3 :
                              nombre.Contains("B") ? 2 : 1;

            for (int i = 0; i < movimientos; i++)
            {
                var movimientoDto = new
                {
                    ProductoId = productoId,
                    ProveedorId = proveedorId,
                    Cantidad = 1,
                    TipoMovimiento = TipoMovimiento.Entrada
                };

                await Client.PostAsJsonAsync("/api/MovimientosStock", movimientoDto);
            }
        }

        return proveedores;
    }



    [Fact]
    public async Task GetTotalProveedores_DebeRetornarCantidadCorrecta()
    {
        // Arrange
        var proveedores = await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProveedorStat/total-proveedores");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var total = await response.Content.ReadFromJsonAsync<int>();
        total.Should().Be(3);
    }

    [Fact]
    public async Task GetProveedorConMasProductos_DebeRetornarProveedorA()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProveedorStat/proveedor-mas-activo");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var proveedor = await response.Content.ReadFromJsonAsync<ProveedorResponseDto>();

        proveedor.Should().NotBeNull();
        proveedor!.Nombre.Should().Be("Proveedor A");
    }


    [Fact]
    public async Task GetProductosPorProveedor_DebeRetornarDistribucionCorrecta()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProveedorStat/productos-por-proveedor");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var distribucion = await response.Content.ReadFromJsonAsync<List<DistribucionProveedorDto>>();

        distribucion.Should().NotBeNull();
        distribucion.Should().HaveCount(3);

        var proveedorA = distribucion!.First(d => d.NombreProveedor == "Proveedor A");
        proveedorA.CantidadProductos.Should().Be(3);

        var proveedorB = distribucion.First(d => d.NombreProveedor == "Proveedor B");
        proveedorB.CantidadProductos.Should().Be(2);

        var proveedorC = distribucion.First(d => d.NombreProveedor == "Proveedor C");
        proveedorC.CantidadProductos.Should().Be(1);
    }

    [Fact]
    public async Task GetValorInventarioPorProveedor_DebeCalcularCorrectamente()
    {
        // Arrange
        await SeedTestDataAsync();

        // Act
        var response = await Client.GetAsync("/api/ProveedorStat/valor-inventario-por-proveedor");

        // Assert
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        var valores = await response.Content.ReadFromJsonAsync<List<DistribucionValorProveedorDto>>();

        valores.Should().NotBeNull();
        valores.Should().HaveCount(3);

        // Proveedor A: (50*1000) + (100*50) + (80*80) = 50000 + 5000 + 6400 = 61400
        var proveedorA = valores!.First(v => v.NombreProveedor == "Proveedor A");
        proveedorA.ValorTotal.Should().BeGreaterThan(0);

        // Proveedor B: (30*1200) + (40*500) = 36000 + 20000 = 56000
        var proveedorB = valores.First(v => v.NombreProveedor == "Proveedor B");
        proveedorB.ValorTotal.Should().BeGreaterThan(0);

        // Proveedor C: (200*10) = 2000
        var proveedorC = valores.First(v => v.NombreProveedor == "Proveedor C");
        proveedorC.ValorTotal.Should().BeGreaterThan(0);
    }
}