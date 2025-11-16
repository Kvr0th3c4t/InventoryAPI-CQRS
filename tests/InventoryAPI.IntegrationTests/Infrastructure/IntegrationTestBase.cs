using InventoryAPI.Data;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Testcontainers.MsSql;
using Microsoft.Extensions.DependencyInjection;

namespace InventoryAPI.IntegrationTests.Infrastructure;

public class IntegrationTestsBase : IAsyncLifetime
{
    private readonly MsSqlContainer _msSqlContainer;
    protected readonly HttpClient Client;
    protected readonly WebApplicationFactory<Program> Factory;

    public IntegrationTestsBase()
    {
        _msSqlContainer = new MsSqlBuilder()
            .WithImage("mcr.microsoft.com/mssql/server:2022-latest")
            .WithPassword("Test@12345")
            .Build();

        Factory = new WebApplicationFactory<Program>()
            .WithWebHostBuilder(builder =>
            {
                builder.ConfigureServices(services =>
                {
                    var descriptor = services.SingleOrDefault(
                        d => d.ServiceType == typeof(DbContextOptions<ApplicationDbContext>));

                    if (descriptor != null)
                    {
                        services.Remove(descriptor);
                    }

                    // NO llamar a GetConnectionString aquí
                    services.AddDbContext<ApplicationDbContext>(options =>
                    {
                        // Usar una lambda lazy
                        options.UseSqlServer(_msSqlContainer.GetConnectionString());
                    });
                });
            });

        Client = Factory.CreateClient();
    }

    public async Task InitializeAsync()
    {
        await _msSqlContainer.StartAsync();

        using var scope = Factory.Services.CreateScope();
        var db = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
        await db.Database.MigrateAsync();
    }

    public async Task DisposeAsync()  // ← Volver a Task
    {
        await _msSqlContainer.DisposeAsync();
        Client.Dispose();
        await Factory.DisposeAsync();
    }
}