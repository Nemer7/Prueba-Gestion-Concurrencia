namespace Prueba_Gestion_Concurrencia.Events
{
    public interface IEventBus
    {
        void Publish<T>(T eventItem);
        Task ProcessEventsAsync(CancellationToken stoppingToken);
    }
}
