namespace BusTicketDemoApp.Models.DTOs
{
    public class LoginRequest
    {
        public string UserName { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
    }
    
    public class ApiResponse<T>
    {
        public string Message { get; set; } = string.Empty;
        public bool Result { get; set; }
        public T? Data { get; set; }
    }
    
    public class BusBookingRequest
    {
        public int BookingId { get; set; }
        public int CustId { get; set; }
        public DateTime BookingDate { get; set; }
        public int ScheduleId { get; set; }
        public List<BusBookingPassengerRequest> BusBookingPassengers { get; set; } = new List<BusBookingPassengerRequest>();
    }
    
    public class BusBookingPassengerRequest
    {
        public int PassengerId { get; set; }
        public int BookingId { get; set; }
        public string PassengerName { get; set; } = string.Empty;
        public int Age { get; set; }
        public string Gender { get; set; } = string.Empty;
        public int SeatNo { get; set; }
    }
    
    public class BusBookingResponse
    {
        public int BookingId { get; set; }
        public int CustId { get; set; }
        public DateTime BookingDate { get; set; }
        public int ScheduleId { get; set; }
    }

    public class BusSearchResult
    {
        public int AvailableSeats { get; set; }
        public int TotalSeats { get; set; }
        public decimal Price { get; set; }
        public DateTime ArrivalTime { get; set; }
        public int ScheduleId { get; set; }
        public DateTime DepartureTime { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string BusVehicleNo { get; set; } = string.Empty;
        public string FromLocationName { get; set; } = string.Empty;
        public string ToLocationName { get; set; } = string.Empty;
        public string VendorName { get; set; } = string.Empty;
        public DateTime ScheduleDate { get; set; }
        public int VendorId { get; set; }
    }

    public class BookingDetailsDto
    {
        public int BookingId { get; set; }
        public string BusName { get; set; } = string.Empty;
        public string FromLocation { get; set; } = string.Empty;
        public string ToLocation { get; set; } = string.Empty;
        public DateTime TravelDate { get; set; }
        public string VendorName { get; set; } = string.Empty;
        public string CustomerName { get; set; } = string.Empty;
        public string EmailId { get; set; } = string.Empty;
        public string BusVehicleNo { get; set; } = string.Empty;
        public List<int> SeatNos { get; set; } = new List<int>();
        public int VendorId { get; set; }
    }
}
