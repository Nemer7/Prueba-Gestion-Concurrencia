using Prueba_Gestion_Concurrencia.DTOs;
using Prueba_Gestion_Concurrencia.Modls;

namespace Prueba_Gestion_Concurrencia.Services
{
    public interface IReservationService
    {
        Task<bool> ReserveRoomAsync(Reservation reservation);
        Task<Reservation> GetReservationAsync(int id);
        Task<bool> CheckRoomAvailabilityAsync(int roomId, DateTime date);
        Task<IEnumerable<Reservation>> GetAllReservationsAsync();
        Task<bool> CancelReservationAsync(int id);
        Task<IEnumerable<Reservation>> GetRoomReservationsAsync(int roomId, DateTime? startDate, DateTime? endDate);

    }
}
