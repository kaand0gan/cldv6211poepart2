using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace EventEasePOE.Models
{
    [Table("Event")]
    public partial class Event
    {
        public Event()
        {
            Bookings = new HashSet<Booking>();
        }

        public int EventId { get; set; }

        [Required]
        [StringLength(100)]
        public string EventName { get; set; }

        [Column(TypeName = "date")]
        public DateTime? EventDate { get; set; }

        [StringLength(255)]
        public string Description { get; set; }

        public int? VenueId { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }

        public virtual Venue Venue { get; set; }
    }
}