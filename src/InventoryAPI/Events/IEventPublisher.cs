namespace InventoryAPI.Events;

public interface IEventPublisher
{
    void Publish<T>(T evento) where T : class;
}