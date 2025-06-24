using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicketDemoApp.Models
{
    public class BusBookingPassenger
    {
        [Key]
        public int PassengerId { get; set; }
        
        public int BookingId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string PassengerName { get; set; } = string.Empty;
        
        [Range(1, 120)]
        public int Age { get; set; }
        
        [Required]
        [StringLength(10)]
        public string Gender { get; set; } = string.Empty;
        
        [Range(1, 100)]
        public int SeatNo { get; set; }
        
        [ForeignKey("BookingId")]
        public virtual BusBooking? BusBooking { get; set; }
    }
}
