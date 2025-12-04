using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.IntegrationTests.Infrastructure;

namespace InventoryAPI.IntegrationTests.Tests.MovimientosStock;

public class MovimientoStockTests : IntegrationTestsBase
{
    [Fact]
    public async Task Create_MovimientoEntrada_ReturnsCreatedMovimiento()
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

        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var IdProveedor = createdProveedor!.Id;

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

        var createMovimientoDto = new CreateMovimientoStockDto
        {
            ProductoId = IdProducto,
            ProveedorId = IdProveedor,
            Tipo = Enums.TipoMovimiento.Entrada,
            Cantidad = 20,
            Razon = "Entrada inicial"
        };

        // When
        var response = await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);
        var movimiento = await response.Content.ReadFromJsonAsync<MovimientoStockResponseDto>();


        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        movimiento!.Id.Should().BeGreaterThan(0);
        movimiento.ProductoId.Should().Be(IdProducto);
        movimiento.ProductoNombre.Should().Be(createProductoDto.Nombre);
        movimiento.ProveedorId.Should().Be(IdProveedor);
        movimiento.ProveedorNombre.Should().Be(proveedorDto.Nombre);
        movimiento.Tipo.Should().Be(createMovimientoDto.Tipo);
        movimiento.Cantidad.Should().Be(createMovimientoDto.Cantidad);
        movimiento.Razon.Should().Be(createMovimientoDto.Razon);

    }

    [Fact]
    public async Task Create_MovimientoSalida_ReturnsCreatedMovimiento()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var categoriaId = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portátil",
            CategoriaId = categoriaId,
            ProveedorId = null,
            StockActual = 100,
            StockMinimo = 20,
            Precio = 999.99m
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var productoId = createdProducto!.Id;

        var createMovimientoDto = new CreateMovimientoStockDto
        {
            ProductoId = productoId,
            ProveedorId = null,
            Tipo = Enums.TipoMovimiento.Salida,
            Cantidad = 20,
            Razon = "Perdida de materiales"
        };

        // When
        var response = await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);
        var movimiento = await response.Content.ReadFromJsonAsync<MovimientoStockResponseDto>();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);
        movimiento.Should().NotBeNull();
        movimiento!.Id.Should().BeGreaterThan(0);
        movimiento.ProductoId.Should().Be(productoId);
        movimiento.ProveedorId.Should().BeNull();
        movimiento.Tipo.Should().Be(Enums.TipoMovimiento.Salida);
        movimiento.Cantidad.Should().Be(20);
        movimiento.Razon.Should().Be(createMovimientoDto.Razon);
        movimiento.ProductoNombre.Should().Be(createProductoDto.Nombre);

        var productoActualizadoResponse = await Client.GetAsync($"/api/Productos/{productoId}");
        var productoActualizado = await productoActualizadoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();

        productoActualizado!.StockActual.Should().Be(80);
    }

    [Fact]
    public async Task GetAll_Movimientos_ReturnsMovimientoList()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var categoriaId = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Portátil",
            CategoriaId = categoriaId,
            ProveedorId = null,
            StockActual = 100,
            StockMinimo = 20,
            Precio = 999.99m
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var productoId = createdProducto!.Id;

        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor@test.com",
            Telefono = "123456789"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var proveedorId = createdProveedor!.Id;

        await Client.PostAsJsonAsync("/api/MovimientosStock", new CreateMovimientoStockDto
        {
            ProductoId = productoId,
            ProveedorId = null,
            Tipo = Enums.TipoMovimiento.Salida,
            Cantidad = 20,
            Razon = "Perdida de materiales"
        });

        await Client.PostAsJsonAsync("/api/MovimientosStock", new CreateMovimientoStockDto
        {
            ProductoId = productoId,
            ProveedorId = proveedorId,
            Tipo = Enums.TipoMovimiento.Entrada,
            Cantidad = 200,
            Razon = "Perdida de materiales"
        });

        // When
        var response = await Client.GetAsync("/api/MovimientosStock");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var movimientos = await response.Content.ReadFromJsonAsync<List<MovimientoStockResponseDto>>();
        movimientos.Should().NotBeNull();
        movimientos.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_Movimiento_ReturnsMovimiento()
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

        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var IdProveedor = createdProveedor!.Id;

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

        var createMovimientoDto = new CreateMovimientoStockDto
        {
            ProductoId = IdProducto,
            ProveedorId = IdProveedor,
            Tipo = Enums.TipoMovimiento.Entrada,
            Cantidad = 20,
            Razon = "Entrada inicial"
        };

        var createMovimientoResponse = await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);
        var createdMovimiento = await createMovimientoResponse.Content.ReadFromJsonAsync<MovimientoStockResponseDto>();
        var IdMovimiento = createdMovimiento!.Id;

        // When
        var response = await Client.GetAsync($"/api/MovimientosStock/{IdMovimiento}");
        var movimiento = await response.Content.ReadFromJsonAsync<MovimientoStockResponseDto>();

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);
        movimiento.Should().NotBeNull();
        movimiento!.Id.Should().BeGreaterThan(0);
        movimiento.ProductoId.Should().Be(IdProducto);
        movimiento.ProveedorId.Should().Be(IdProveedor);
        movimiento.ProveedorNombre.Should().Be(proveedorDto.Nombre);
        movimiento.Tipo.Should().Be(Enums.TipoMovimiento.Entrada);
        movimiento.Cantidad.Should().Be(20);
        movimiento.Razon.Should().Be(createMovimientoDto.Razon);
        movimiento.ProductoNombre.Should().Be(createProductoDto.Nombre);
    }

    [Fact]
    public async Task Create_MovimientoEntrada_WithoutProveedor_ReturnsBadRequest()
    {
        // Given
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var categoriaId = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Tablet",
            CategoriaId = categoriaId,
            ProveedorId = null,
            StockActual = 50,
            StockMinimo = 10,
            Precio = 299.99m
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var productoId = createdProducto!.Id;

        var createMovimientoDto = new CreateMovimientoStockDto
        {
            ProductoId = productoId,
            ProveedorId = null,
            Tipo = Enums.TipoMovimiento.Entrada,
            Cantidad = 20,
            Razon = "Intento de entrada sin proveedor"
        };

        // When 
        var response = await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);

        // Then 
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_MovimientoSalida_WithInsufficientStock_ReturnsBadRequest()
    {
        // Given 
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var categoriaId = createdCategoria!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Mouse",
            CategoriaId = categoriaId,
            ProveedorId = null,
            StockActual = 10,
            StockMinimo = 5,
            Precio = 19.99m
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var productoId = createdProducto!.Id;

        var createMovimientoDto = new CreateMovimientoStockDto
        {
            ProductoId = productoId,
            ProveedorId = null,
            Tipo = Enums.TipoMovimiento.Salida,
            Cantidad = 50,
            Razon = "Intento de salida con stock insuficiente"
        };

        // When
        var response = await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);

        // Then 
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }

    [Fact]
    public async Task Create_MovimientoEntrada_UpdatesProductStock()
    {
        // Given 
        var createCategoriaDto = new CreateCategoriaDto
        {
            Nombre = "Electrónica",
            Descripcion = "Productos electrónicos"
        };

        var createCategoriaResponse = await Client.PostAsJsonAsync("/api/Categoria", createCategoriaDto);
        var createdCategoria = await createCategoriaResponse.Content.ReadFromJsonAsync<CategoriaResponseDto>();
        var categoriaId = createdCategoria!.Id;

        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor Tech",
            Email = "tech@proveedor.com",
            Telefono = "555-1234"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var proveedorId = createdProveedor!.Id;

        var createProductoDto = new CreateProductoDto
        {
            Nombre = "Teclado",
            CategoriaId = categoriaId,
            ProveedorId = null,
            StockActual = 50,
            StockMinimo = 10,
            Precio = 79.99m
        };

        var createProductoResponse = await Client.PostAsJsonAsync("/api/Productos", createProductoDto);
        var createdProducto = await createProductoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();
        var productoId = createdProducto!.Id;

        var createMovimientoDto = new CreateMovimientoStockDto
        {
            ProductoId = productoId,
            ProveedorId = proveedorId,
            Tipo = Enums.TipoMovimiento.Entrada,
            Cantidad = 30,
            Razon = "Reposición de stock"
        };

        // When 
        var response = await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);

        // Then 
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var productoActualizadoResponse = await Client.GetAsync($"/api/Productos/{productoId}");
        var productoActualizado = await productoActualizadoResponse.Content.ReadFromJsonAsync<ProductoResponseDto>();

        productoActualizado!.StockActual.Should().Be(80);
    }
}