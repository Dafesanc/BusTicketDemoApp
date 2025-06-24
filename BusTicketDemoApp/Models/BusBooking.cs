using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace BusTicketDemoApp.Models
{
    public class BusBooking
    {
        [Key]
        public int BookingId { get; set; }
        
        public int CustId { get; set; }
        
        public DateTime BookingDate { get; set; } = DateTime.UtcNow;
        
        public int ScheduleId { get; set; }
        
        [ForeignKey("ScheduleId")]
        public virtual BusSchedule? BusSchedule { get; set; }
        
        public virtual ICollection<BusBookingPassenger> BusBookingPassengers { get; set; } = new List<BusBookingPassenger>();
    }
}
