using System.ComponentModel.DataAnnotations;

namespace BusTicketDemoApp.Models
{
    public class BusLocation
    {
        [Key]
        public int LocationId { get; set; }
        public string LocationName { get; set; } = string.Empty;
        public string Code { get; set; } = string.Empty;
    }
}
