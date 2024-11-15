using Prueba_Gestion_Concurrencia.Events;

namespace Prueba_Gestion_Concurrencia.BackgroundServices
{
    public class ReservationEventWorker : BackgroundService
    {
        private readonly IEventBus _eventBus;

        public ReservationEventWorker(IEventBus eventBus)
        {
            _eventBus = eventBus;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await _eventBus.ProcessEventsAsync(stoppingToken);
        }
    }
}
