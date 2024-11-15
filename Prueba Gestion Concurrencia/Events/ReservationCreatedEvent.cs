using Prueba_Gestion_Concurrencia.Modls;

namespace Prueba_Gestion_Concurrencia.Events
{
    public class ReservationCreatedEvent
    {
        public Reservation Reservation { get; set; }

        public ReservationCreatedEvent(Reservation reservation)
        {
            Reservation = reservation;
        }
    }
}
