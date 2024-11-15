using System.ComponentModel.DataAnnotations;

namespace Prueba_Gestion_Concurrencia.Modls
{
    public class Reservation
    {
        [Key]
        public int Id { get; set; }

        [Required]
        public int RoomId { get; set; }

        [Required]
        public DateTime ReservationDate { get; set; }

        [Timestamp]
        public byte[] RowVersion { get; set; }
    }
}
