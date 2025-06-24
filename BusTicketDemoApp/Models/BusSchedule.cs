using System.ComponentModel.DataAnnotations;

namespace BusTicketDemoApp.Models
{
    public class BusSchedule
    {
        [Key]
        public int ScheduleId { get; set; }
        
        public int VendorId { get; set; }
        
        [Required]
        [StringLength(100)]
        public string BusName { get; set; } = string.Empty;
        
        [Required]
        [StringLength(20)]
        public string BusVehicleNo { get; set; } = string.Empty;
        
        public int FromLocation { get; set; }
        
        public int ToLocation { get; set; }
        
        public DateTime DepartureTime { get; set; }
        
        public DateTime ArrivalTime { get; set; }
        
        public DateTime ScheduleDate { get; set; }
        
        [Range(0, double.MaxValue)]
        public decimal Price { get; set; }
        
        [Range(1, 100)]
        public int TotalSeats { get; set; }
    }
}
