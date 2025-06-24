using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BusTicketDemoApp.Data;
using BusTicketDemoApp.Models;
using BusTicketDemoApp.Models.DTOs;

namespace BusTicketDemoApp.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BusBookingController : ControllerBase
    {
        private readonly BusTicketDbContext _context;
        private readonly ILogger<BusBookingController> _logger;

        public BusBookingController(BusTicketDbContext context, ILogger<BusBookingController> logger)
        {
            _context = context;
            _logger = logger;
        }

        [HttpPost("login")]
        public async Task<ActionResult<ApiResponse<string>>> Login([FromBody] LoginRequest request)
        {
            try
            {
                var user = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == request.UserName && u.Password == request.Password);

                if (user == null)
                {
                    return Ok(new ApiResponse<string>
                    {
                        Message = "Invalid username or password",
                        Result = false,
                        Data = null
                    });
                }

                // In a real app, you would generate a JWT token here
                var token = $"fake-jwt-token-for-{user.UserName}-{user.UserId}";
                
                return Ok(new ApiResponse<string>
                {
                    Message = "Login successful",
                    Result = true,
                    Data = token
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error during login");
                return Ok(new ApiResponse<string>
                {
                    Message = "An error occurred during login",
                    Result = false,
                    Data = null
                });
            }
        }

        [HttpGet("GetAllUsers")]
        public async Task<ActionResult<ApiResponse<List<User>>>> GetAllUsers()
        {
            try
            {
                var users = await _context.Users.ToListAsync();
                
                return Ok(new ApiResponse<List<User>>
                {
                    Message = "Users retrieved successfully",
                    Result = true,
                    Data = users
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all users");
                return Ok(new ApiResponse<List<User>>
                {
                    Message = "An error occurred while retrieving users",
                    Result = false,
                    Data = null
                });
            }
        }

        [HttpPost("AddNewUser")]
        public async Task<ActionResult<ApiResponse<User>>> AddNewUser([FromBody] User user)
        {
            try
            {
                // Check if user already exists
                var existingUser = await _context.Users
                    .FirstOrDefaultAsync(u => u.UserName == user.UserName || u.EmailId == user.EmailId);

                if (existingUser != null)
                {
                    return Ok(new ApiResponse<User>
                    {
                        Message = "User with this username or email already exists",
                        Result = false,
                        Data = null
                    });
                }

                user.CreatedDate = DateTime.UtcNow;
                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<User>
                {
                    Message = "User added successfully",
                    Result = true,
                    Data = user
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding new user");
                return Ok(new ApiResponse<User>
                {
                    Message = "An error occurred while adding the user",
                    Result = false,
                    Data = null
                });
            }
        }

        [HttpPost("UpdateUser")]
        public async Task<ActionResult<ApiResponse<User>>> UpdateUser([FromBody] User user)
        {
            try
            {
                var existingUser = await _context.Users.FindAsync(user.UserId);
                if (existingUser == null)
                {
                    return Ok(new ApiResponse<User>
                    {
                        Message = "User not found",
                        Result = false,
                        Data = null
                    });
                }

                // Update properties
                existingUser.UserName = user.UserName;
                existingUser.EmailId = user.EmailId;
                existingUser.FullName = user.FullName;
                existingUser.Role = user.Role;
                existingUser.Password = user.Password;
                existingUser.ProjectName = user.ProjectName;
                existingUser.RefreshToken = user.RefreshToken;
                existingUser.RefreshTokenExpiryTime = user.RefreshTokenExpiryTime;

                await _context.SaveChangesAsync();

                return Ok(new ApiResponse<User>
                {
                    Message = "User updated successfully",
                    Result = true,
                    Data = existingUser
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating user");
                return Ok(new ApiResponse<User>
                {
                    Message = "An error occurred while updating the user",
                    Result = false,
                    Data = null
                });
            }
        }

        [HttpGet("GetBusScheduleById")]
        public async Task<ActionResult<BusSchedule?>> GetBusScheduleById([FromQuery] int id)
        {
            try
            {
                var schedule = await _context.BusSchedules.FindAsync(id);
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bus schedule");
                return BadRequest("An error occurred while retrieving the bus schedule");
            }
        }

        [HttpGet("getBookedSeats")]
        public async Task<ActionResult<List<int>>> GetBookedSeats([FromQuery] int shceduleId)
        {
            try
            {
                var bookedSeats = await _context.BusBookingPassengers
                    .Where(p => _context.BusBookings
                        .Any(b => b.BookingId == p.BookingId && b.ScheduleId == shceduleId))
                    .Select(p => p.SeatNo)
                    .ToListAsync();

                return Ok(bookedSeats);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting booked seats");
                return BadRequest("An error occurred while retrieving booked seats");
            }
        }

        [HttpPost("PostBusBooking")]
        public async Task<ActionResult<BusBookingResponse>> PostBusBooking([FromBody] BusBookingRequest request)
        {
            try
            {
                // Check if schedule exists
                var schedule = await _context.BusSchedules.FindAsync(request.ScheduleId);
                if (schedule == null)
                {
                    return BadRequest("Bus schedule not found");
                }

                // Check if seats are available
                var bookedSeats = await _context.BusBookingPassengers
                    .Where(p => _context.BusBookings
                        .Any(b => b.BookingId == p.BookingId && b.ScheduleId == request.ScheduleId))
                    .Select(p => p.SeatNo)
                    .ToListAsync();

                var requestedSeats = request.BusBookingPassengers.Select(p => p.SeatNo).ToList();
                var conflictingSeats = requestedSeats.Intersect(bookedSeats).ToList();
                
                if (conflictingSeats.Any())
                {
                    return BadRequest($"Seats {string.Join(", ", conflictingSeats)} are already booked");
                }

                // Create booking
                var booking = new BusBooking
                {
                    CustId = request.CustId,
                    BookingDate = request.BookingDate,
                    ScheduleId = request.ScheduleId
                };

                _context.BusBookings.Add(booking);
                await _context.SaveChangesAsync();

                // Add passengers
                foreach (var passengerRequest in request.BusBookingPassengers)
                {
                    var passenger = new BusBookingPassenger
                    {
                        BookingId = booking.BookingId,
                        PassengerName = passengerRequest.PassengerName,
                        Age = passengerRequest.Age,
                        Gender = passengerRequest.Gender,
                        SeatNo = passengerRequest.SeatNo
                    };
                    _context.BusBookingPassengers.Add(passenger);
                }

                await _context.SaveChangesAsync();

                var response = new BusBookingResponse
                {
                    BookingId = booking.BookingId,
                    CustId = booking.CustId,
                    BookingDate = booking.BookingDate,
                    ScheduleId = booking.ScheduleId
                };

                return Ok(response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating bus booking");
                return BadRequest("An error occurred while creating the booking");
            }
        }

        [HttpGet("searchBus")]
        public async Task<ActionResult<List<BusSearchResult>>> SearchBus([FromQuery] int fromLocation, [FromQuery] int toLocation, [FromQuery] DateTime travelDate)
        {
            try
            {
                var travelDateOnly = travelDate.Date;
                
                var busResults = await _context.BusSchedules
                    .Where(s => s.FromLocation == fromLocation && 
                               s.ToLocation == toLocation && 
                               s.ScheduleDate.Date == travelDateOnly)
                    .Select(s => new BusSearchResult
                    {
                        ScheduleId = s.ScheduleId,
                        VendorId = s.VendorId,
                        BusName = s.BusName,
                        BusVehicleNo = s.BusVehicleNo,
                        DepartureTime = s.DepartureTime,
                        ArrivalTime = s.ArrivalTime,
                        ScheduleDate = s.ScheduleDate,
                        Price = s.Price,
                        TotalSeats = s.TotalSeats,
                        AvailableSeats = s.TotalSeats - _context.BusBookingPassengers
                            .Count(p => _context.BusBookings
                                .Any(b => b.BookingId == p.BookingId && b.ScheduleId == s.ScheduleId)),
                        FromLocationName = _context.BusLocations
                            .Where(l => l.LocationId == s.FromLocation)
                            .Select(l => l.LocationName)
                            .FirstOrDefault() ?? "Unknown",
                        ToLocationName = _context.BusLocations
                            .Where(l => l.LocationId == s.ToLocation)
                            .Select(l => l.LocationName)
                            .FirstOrDefault() ?? "Unknown",
                        VendorName = _context.Vendors
                            .Where(v => v.VendorId == s.VendorId)
                            .Select(v => v.VendorName)
                            .FirstOrDefault() ?? "Unknown"
                    })
                    .ToListAsync();

                return Ok(busResults);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error searching for buses");
                return BadRequest("An error occurred while searching for buses");
            }
        }

        [HttpPut("PutBusLocation")]
        public async Task<ActionResult<BusLocation>> PutBusLocation([FromQuery] int id, [FromBody] BusLocation location)
        {
            try
            {
                var existingLocation = await _context.BusLocations.FindAsync(id);
                if (existingLocation == null)
                {
                    return NotFound("Bus location not found");
                }

                existingLocation.LocationName = location.LocationName;
                existingLocation.Code = location.Code;

                await _context.SaveChangesAsync();

                return Ok(existingLocation);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating bus location");
                return BadRequest("An error occurred while updating the bus location");
            }
        }

        [HttpGet("GetAllBusBookings")]
        public async Task<ActionResult<List<BookingDetailsDto>>> GetAllBusBookings([FromQuery] int vendorId)
        {
            try
            {
                var bookings = await _context.BusBookings
                    .Where(b => _context.BusSchedules.Any(s => s.ScheduleId == b.ScheduleId && s.VendorId == vendorId))
                    .Select(b => new BookingDetailsDto
                    {
                        BookingId = b.BookingId,
                        TravelDate = b.BookingDate,
                        VendorId = vendorId,
                        BusName = _context.BusSchedules
                            .Where(s => s.ScheduleId == b.ScheduleId)
                            .Select(s => s.BusName)
                            .FirstOrDefault() ?? "Unknown",
                        BusVehicleNo = _context.BusSchedules
                            .Where(s => s.ScheduleId == b.ScheduleId)
                            .Select(s => s.BusVehicleNo)
                            .FirstOrDefault() ?? "Unknown",
                        FromLocation = _context.BusLocations
                            .Where(l => _context.BusSchedules
                                .Any(s => s.ScheduleId == b.ScheduleId && s.FromLocation == l.LocationId))
                            .Select(l => l.LocationName)
                            .FirstOrDefault() ?? "Unknown",
                        ToLocation = _context.BusLocations
                            .Where(l => _context.BusSchedules
                                .Any(s => s.ScheduleId == b.ScheduleId && s.ToLocation == l.LocationId))
                            .Select(l => l.LocationName)
                            .FirstOrDefault() ?? "Unknown",
                        VendorName = _context.Vendors
                            .Where(v => v.VendorId == vendorId)
                            .Select(v => v.VendorName)
                            .FirstOrDefault() ?? "Unknown",
                        CustomerName = _context.Users
                            .Where(u => u.UserId == b.CustId)
                            .Select(u => u.FullName)
                            .FirstOrDefault() ?? "Unknown",
                        EmailId = _context.Users
                            .Where(u => u.UserId == b.CustId)
                            .Select(u => u.EmailId)
                            .FirstOrDefault() ?? "Unknown",
                        SeatNos = _context.BusBookingPassengers
                            .Where(p => p.BookingId == b.BookingId)
                            .Select(p => p.SeatNo)
                            .ToList()
                    })
                    .ToListAsync();

                return Ok(bookings);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting all bus bookings");
                return BadRequest("An error occurred while retrieving bookings");
            }
        }

        // Helper endpoints for testing and data management
        [HttpPost("AddBusLocation")]
        public async Task<ActionResult<BusLocation>> AddBusLocation([FromBody] BusLocation location)
        {
            try
            {
                _context.BusLocations.Add(location);
                await _context.SaveChangesAsync();
                return Ok(location);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus location");
                return BadRequest("An error occurred while adding the bus location");
            }
        }

        [HttpPost("AddVendor")]
        public async Task<ActionResult<Vendor>> AddVendor([FromBody] Vendor vendor)
        {
            try
            {
                vendor.CreatedDate = DateTime.UtcNow;
                _context.Vendors.Add(vendor);
                await _context.SaveChangesAsync();
                return Ok(vendor);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding vendor");
                return BadRequest("An error occurred while adding the vendor");
            }
        }

        [HttpPost("AddBusSchedule")]
        public async Task<ActionResult<BusSchedule>> AddBusSchedule([FromBody] BusSchedule schedule)
        {
            try
            {
                _context.BusSchedules.Add(schedule);
                await _context.SaveChangesAsync();
                return Ok(schedule);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error adding bus schedule");
                return BadRequest("An error occurred while adding the bus schedule");
            }
        }

        [HttpGet("GetAllBusLocations")]
        public async Task<ActionResult<List<BusLocation>>> GetAllBusLocations()
        {
            try
            {
                var locations = await _context.BusLocations.ToListAsync();
                return Ok(locations);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bus locations");
                return BadRequest("An error occurred while retrieving bus locations");
            }
        }

        [HttpGet("GetAllVendors")]
        public async Task<ActionResult<List<Vendor>>> GetAllVendors()
        {
            try
            {
                var vendors = await _context.Vendors.ToListAsync();
                return Ok(vendors);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting vendors");
                return BadRequest("An error occurred while retrieving vendors");
            }
        }

        [HttpGet("GetAllBusSchedules")]
        public async Task<ActionResult<List<BusSchedule>>> GetAllBusSchedules()
        {
            try
            {
                var schedules = await _context.BusSchedules.ToListAsync();
                return Ok(schedules);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error getting bus schedules");
                return BadRequest("An error occurred while retrieving bus schedules");
            }
        }
    }
}
