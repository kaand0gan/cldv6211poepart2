using System.Data.Entity;

namespace EventEasePOE.Models
{
    public partial class EventEaseContext : DbContext
    {
        public EventEaseContext()
            : base("name=EventEaseContext")
        {
        }

        public virtual DbSet<Booking> Bookings { get; set; }
        public virtual DbSet<Event> Events { get; set; }
        public virtual DbSet<Venue> Venues { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            // Event -> Bookings (1-many), Required
            modelBuilder.Entity<Event>()
                .HasMany(e => e.Bookings)
                .WithRequired(b => b.Event)
                .HasForeignKey(b => b.EventId)
                .WillCascadeOnDelete(false);

            // Venue -> Bookings (1-many), Required
            modelBuilder.Entity<Venue>()
                .HasMany(v => v.Bookings)
                .WithRequired(b => b.Venue)
                .HasForeignKey(b => b.VenueId)
                .WillCascadeOnDelete(false);
        }
    }
}
