namespace BusTicketDemoApp.Models
{
    public class Vendor
    {
        public int VendorId { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Phone { get; set; } = string.Empty;
        public DateTime CreatedDate { get; set; }
    }
}
