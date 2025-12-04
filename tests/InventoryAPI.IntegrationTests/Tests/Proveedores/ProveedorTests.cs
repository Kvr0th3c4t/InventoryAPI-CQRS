using System.Net;
using System.Net.Http.Json;
using FluentAssertions;
using InventoryAPI.Dtos.CategoriaDtos;
using InventoryAPI.Dtos.MovimientoStockDtos;
using InventoryAPI.Dtos.ProductoDtos;
using InventoryAPI.Dtos.ProveedorDtos;
using InventoryAPI.IntegrationTests.Infrastructure;
using InventoryAPI.Models;

namespace InventoryAPI.IntegrationTests.Tests.Proveeores;

public class ProveedorTests : IntegrationTestsBase
{
    [Fact]
    public async Task Create_Proveedor_ReturnCreatedProveedor()
    {
        //Given
        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        };

        //When
        var response = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);

        //Then
        response.StatusCode.Should().Be(HttpStatusCode.Created);

        var proveedor = await response.Content.ReadFromJsonAsync<ProveedorResponseDto>();

        proveedor.Should().NotBeNull();
        proveedor!.Id.Should().BeGreaterThan(0);
        proveedor.Nombre.Should().Be(proveedorDto.Nombre);
        proveedor.Email.Should().Be(proveedorDto.Email);
        proveedor.Telefono.Should().Be(proveedorDto.Telefono);

    }

    [Fact]
    public async Task GetAll_Proveedores_ReturnProveedorList()
    {
        // Given

        await Client.PostAsJsonAsync("/api/Proveedor", new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        });

        await Client.PostAsJsonAsync("/api/Proveedor", new CreateProveedorDto
        {
            Nombre = "Proveedor 2",
            Email = "proveedor2@example.com",
            Telefono = "Telefono"
        });

        // When
        var response = await Client.GetAsync("/api/Proveedor");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var proveedores = await response.Content.ReadFromJsonAsync<List<ProveedorResponseDto>>();
        proveedores.Should().NotBeNull();
        proveedores.Should().HaveCount(2);
    }

    [Fact]
    public async Task GetById_Proveedor_ReturnProveedor()
    {
        // Given
        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var IdProveedor = createdProveedor!.Id;

        // When
        var response = await Client.GetAsync($"/api/Proveedor/{IdProveedor}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var proveedor = await response.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        proveedor.Should().NotBeNull();
        proveedor!.Id.Should().Be(IdProveedor);
        proveedor.Nombre.Should().Be(proveedorDto.Nombre);
        proveedor.Email.Should().Be(proveedorDto.Email);
        proveedor.Telefono.Should().Be(proveedorDto.Telefono);

    }

    [Fact]
    public async Task Update_Proveedor_ReturnUpdatedProveedor()
    {
        // Given
        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var IdProveedor = createdProveedor!.Id;

        var updateDto = new UpdateProveedorDto
        {
            Nombre = "Proveedor modificado",
            Email = "proveedor.modificado@example.com",
            Telefono = "Telefono mdificado"
        };

        // When
        var response = await Client.PutAsJsonAsync($"/api/Proveedor/{IdProveedor}", updateDto);

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.OK);

        var proveedor = await response.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        proveedor.Should().NotBeNull();
        proveedor!.Id.Should().Be(IdProveedor);
        proveedor.Nombre.Should().Be(updateDto.Nombre);
        proveedor.Email.Should().Be(updateDto.Email);
        proveedor.Telefono.Should().Be(updateDto.Telefono);

    }

    [Fact]
    public async Task Delete_Proveedor_ReturnNoContent()
    {
        // Given
        var proveedorDto = new CreateProveedorDto
        {
            Nombre = "Proveedor 1",
            Email = "proveedor1@example.com",
            Telefono = "134134"
        };

        var createProveedorResponse = await Client.PostAsJsonAsync("/api/Proveedor", proveedorDto);
        var createdProveedor = await createProveedorResponse.Content.ReadFromJsonAsync<ProveedorResponseDto>();
        var IdProveedor = createdProveedor!.Id;


        // When
        var response = await Client.DeleteAsync($"/api/Proveedor/{IdProveedor}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.NoContent);

        var checkDelete = await Client.GetAsync($"/api/Proveedor/{IdProveedor}");

        checkDelete.StatusCode.Should().Be(HttpStatusCode.NotFound);
    }

    [Fact]
    public async Task Delete_Proveedor_WithMovimientos_ReturnsBadRequest()
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
        await Client.PostAsJsonAsync("/api/MovimientosStock", createMovimientoDto);

        // When
        var response = await Client.DeleteAsync($"/api/Proveedor/{IdProveedor}");

        // Then
        response.StatusCode.Should().Be(HttpStatusCode.BadRequest);
    }
}