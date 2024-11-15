using Microsoft.EntityFrameworkCore;
using Prueba_Gestion_Concurrencia.Modls;

namespace Prueba_Gestion_Concurrencia.Data
{
    public class ApplicationDbContext : DbContext
    {
        public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options)
            : base(options)
        {
        }

        public DbSet<Reservation> Reservations { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Reservation>()
                .Property(e => e.RowVersion)
                .IsRowVersion();

          
            modelBuilder.Entity<Reservation>()
                .HasIndex(e => new { e.RoomId, e.ReservationDate });
        }
    }
}
