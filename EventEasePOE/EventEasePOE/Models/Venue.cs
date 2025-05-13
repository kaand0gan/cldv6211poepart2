using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Web;
using System.ComponentModel;

namespace EventEasePOE.Models
{
    [Table("Venue")]
    public partial class Venue
    {
        public Venue()
        {
            Bookings = new HashSet<Booking>();
            Events = new HashSet<Event>();
        }

        public int VenueId { get; set; }

        [Required]
        [StringLength(100)]
        public string VenueName { get; set; }

        [StringLength(100)]
        public string Location { get; set; }

        public int? Capacity { get; set; }

        [StringLength(255)]
        public string ImageUrl { get; set; }

        public virtual ICollection<Booking> Bookings { get; set; }
        public virtual ICollection<Event> Events { get; set; }

        // Not mapped property for image upload
        [NotMapped]
        [DisplayName("Upload Image")]
        public HttpPostedFileBase ImageFile { get; set; }
    }
}