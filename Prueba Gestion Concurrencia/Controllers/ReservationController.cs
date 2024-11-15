using Microsoft.AspNetCore.Mvc;
using Prueba_Gestion_Concurrencia.Modls;
using Prueba_Gestion_Concurrencia.Services;
using Prueba_Gestion_Concurrencia.DTOs;

namespace Prueba_Gestion_Concurrencia.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ReservationController : ControllerBase
    {
        private readonly IReservationService _reservationService;

        public ReservationController(IReservationService reservationService)
        {
            _reservationService = reservationService;
        }

        [HttpPost("reserve-room")]
        public async Task<IActionResult> CreateReservation([FromBody] CreateReservationDto dto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var reservation = new Reservation
            {
                RoomId = dto.RoomId,
                ReservationDate = dto.ReservationDate
            };

            try
            {
                var result = await _reservationService.ReserveRoomAsync(reservation);
                if (!result)
                {
                    return Conflict("Conflicto de concurrencia. Intente nuevamente.");
                }
                return CreatedAtAction(nameof(GetReservation), new { id = reservation.Id }, reservation);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<Reservation>> GetReservation(int id)
        {
            var reservation = await _reservationService.GetReservationAsync(id);

            if (reservation == null)
            {
                return NotFound();
            }

            return Ok(reservation);
        }

        [HttpGet("room/{roomId}/availability")]
        public async Task<ActionResult<bool>> CheckRoomAvailability(int roomId, [FromQuery] DateTime date)
        {
            var isAvailable = await _reservationService.CheckRoomAvailabilityAsync(roomId, date);
            return Ok(new { isAvailable = isAvailable });
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetAllReservations()
        {
            var reservations = await _reservationService.GetAllReservationsAsync();
            return Ok(reservations);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> CancelReservation(int id)
        {
            try
            {
                var result = await _reservationService.CancelReservationAsync(id);
                if (!result)
                {
                    return NotFound("Reservación no encontrada");
                }

                return NoContent();
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("room/{roomId}/reservations")]
        public async Task<ActionResult<IEnumerable<Reservation>>> GetRoomReservations(int roomId, [FromQuery] DateTime? startDate, [FromQuery] DateTime? endDate)
        {
            var reservations = await _reservationService.GetRoomReservationsAsync(roomId, startDate, endDate);
            return Ok(reservations);
        }
    }
}