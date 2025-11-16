using InventoryAPI.Events;

namespace InventoryAPI.Services;

public class ConsoleEventPublisher : IEventPublisher
{
    public void Publish<T>(T evento) where T : class
    {
        Console.WriteLine($"📢 Evento publicado: {typeof(T).Name}");
        Console.WriteLine($"   Datos: {System.Text.Json.JsonSerializer.Serialize(evento)}");
    }
}