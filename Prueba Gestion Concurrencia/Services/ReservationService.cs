using Microsoft.EntityFrameworkCore;
using Prueba_Gestion_Concurrencia.Data;
using Prueba_Gestion_Concurrencia.DTOs;
using Prueba_Gestion_Concurrencia.Events;
using Prueba_Gestion_Concurrencia.Modls;

namespace Prueba_Gestion_Concurrencia.Services
{
    public class ReservationService : IReservationService
    {
        private readonly ApplicationDbContext _context;
        private readonly IEventBus _eventBus;

        public ReservationService(ApplicationDbContext context, IEventBus eventBus)
        {
            _context = context;
            _eventBus = eventBus;
        }

        public async Task<bool> ReserveRoomAsync(Reservation reservation)
        {
            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Reservations.Add(reservation);
                await _context.SaveChangesAsync();

                var reservationCreatedEvent = new ReservationCreatedEvent(reservation);
                _eventBus.Publish(reservationCreatedEvent);

                await transaction.CommitAsync();
                return true;
            }
            catch (DbUpdateConcurrencyException)
            {
                await transaction.RollbackAsync();
                return false;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Implementación de GetReservationAsync
        public async Task<Reservation> GetReservationAsync(int id)
        {
            return await _context.Reservations.FindAsync(id);
        }

        // Implementación de CheckRoomAvailabilityAsync
        public async Task<bool> CheckRoomAvailabilityAsync(int roomId, DateTime date)
        {
            return !await _context.Reservations
                .AnyAsync(r => r.RoomId == roomId && r.ReservationDate.Date == date.Date);
        }

        // Implementación de GetAllReservationsAsync
        public async Task<IEnumerable<Reservation>> GetAllReservationsAsync()
        {
            return await _context.Reservations
                .OrderByDescending(r => r.ReservationDate)
                .ToListAsync();
        }

        // Implementación de CancelReservationAsync
        public async Task<bool> CancelReservationAsync(int id)
        {
            var reservation = await _context.Reservations.FindAsync(id);
            if (reservation == null)
            {
                return false;
            }

            using var transaction = await _context.Database.BeginTransactionAsync();
            try
            {
                _context.Reservations.Remove(reservation);
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
                return true;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        // Implementación de GetRoomReservationsAsync
        public async Task<IEnumerable<Reservation>> GetRoomReservationsAsync(int roomId, DateTime? startDate, DateTime? endDate)
        {
            var query = _context.Reservations.Where(r => r.RoomId == roomId);

            if (startDate.HasValue)
            {
                query = query.Where(r => r.ReservationDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(r => r.ReservationDate <= endDate.Value);
            }

            return await query.OrderBy(r => r.ReservationDate).ToListAsync();
        }
    }
}