using System.Collections.Concurrent;

namespace Prueba_Gestion_Concurrencia.Events
{
    public class InMemoryEventBus : IEventBus
    {
        private readonly ConcurrentQueue<object> _events = new ConcurrentQueue<object>();

        public void Publish<T>(T eventItem)
        {
            _events.Enqueue(eventItem);
        }

        public async Task ProcessEventsAsync(CancellationToken stoppingToken)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                while (_events.TryDequeue(out var eventItem))
                {
                    // Procesar el evento (enviar correo, etc.)
                    await HandleEventAsync(eventItem);
                }
                await Task.Delay(1000, stoppingToken); // Esperar antes de procesar nuevamente
            }
        }

        private Task HandleEventAsync(object eventItem)
        {
            // Aquí puedes implementar el manejo del evento, como enviar un correo
            return Task.CompletedTask;
        }
    }
}
