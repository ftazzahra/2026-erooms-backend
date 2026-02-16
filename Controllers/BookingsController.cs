using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;
using erooms.Data;
using erooms.Models;
using erooms.DTOs;

namespace erooms.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BookingsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public BookingsController(AppDbContext context)
        {
            _context = context;
        }

        // Create booking for user
        [Authorize(Roles = "User")]
        [HttpPost]
        public async Task<IActionResult> CreateBooking(BookingRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = new Booking
            {
                UserId = userId,
                RoomId = dto.RoomId,
                BorrowDate = dto.BorrowDate,
                ReturnDate = dto.ReturnDate,
                Status = "Pending",
                Purpose = dto.Purpose
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingDetail),
                new { id = booking.Id },
                new { message = "Booking created successfully", bookingId = booking.Id });
        }

        // get my bookings
        [Authorize(Roles = "User")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Room) // pastikan include
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    RoomLocation = b.Room.Location, // baru
                    RoomCapacity = b.Room.Capacity, // baru
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate,
                    Status = b.Status,
                    Purpose = b.Purpose
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // get booking detail
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingDetail(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await _context.Bookings
                .Where(b => b.Id == id && b.UserId == userId)
                .Include(b => b.Room) // pastikan include
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    RoomLocation = b.Room.Location, // baru
                    RoomCapacity = b.Room.Capacity, // baru
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate,
                    Status = b.Status,
                    Purpose = b.Purpose
                })
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }


        // Update booking for user
        [Authorize(Roles = "User")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateBooking(int id, BookingRequestDto dto)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            if (booking.Status != "Pending")
                return BadRequest(new { message = "Cannot update approved/rejected booking" });

            booking.RoomId = dto.RoomId;
            booking.BorrowDate = dto.BorrowDate;
            booking.ReturnDate = dto.ReturnDate;
            booking.Purpose = dto.Purpose;

            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking updated successfully" });
        }

        // Delete booking for user
        [Authorize(Roles = "User")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteBooking(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await _context.Bookings
                .FirstOrDefaultAsync(b => b.Id == id && b.UserId == userId);

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            if (booking.Status != "Pending")
                return BadRequest(new { message = "Cannot delete approved/rejected booking" });

            _context.Bookings.Remove(booking);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking deleted successfully" });
        }

        // Get all bookings for admin
        [Authorize(Roles = "Admin")]
        [HttpGet("admin")]
        public async Task<IActionResult> GetAllBookings()
        {
            var bookings = await _context.Bookings
            .Include(b => b.Room)
            .Include(b => b.User)
            .Select(b => new
            {
                b.Id,
                b.UserId,
                UserName = b.User.Username,
                b.RoomId,
                RoomName = b.Room.Name,
                RoomLocation = b.Room.Location,  
                RoomCapacity = b.Room.Capacity,    
                b.BorrowDate,
                b.ReturnDate,
                b.Status,
                Purpose = b.Purpose
            })
            .ToListAsync();


            return Ok(bookings);
        }

        // Update booking status for admin
        [Authorize(Roles = "Admin")]
        [HttpPut("admin/{id}/status")]
        public async Task<IActionResult> UpdateBookingStatus(int id, [FromBody] string status)
        {
            var booking = await _context.Bookings.FindAsync(id);

            if (booking == null)
                return NotFound();

            if (status != "Approved" && status != "Rejected")
                return BadRequest("Status must be Approved or Rejected.");

            booking.Status = status;
            await _context.SaveChangesAsync();

            return Ok("Booking status updated successfully.");
        }

        // Get booking detail for admin
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/{id}")]
        public async Task<IActionResult> GetBookingDetailAdmin(int id)
        {
            var booking = await _context.Bookings
                .Include(b => b.Room)
                .Include(b => b.User)
                .FirstOrDefaultAsync(b => b.Id == id);

            if (booking == null)
                return NotFound();

            return Ok(new
            {
                booking.Id,
                booking.UserId,
                UserName = booking.User.Username,
                booking.RoomId,
                RoomName = booking.Room.Name,
                booking.BorrowDate,
                booking.ReturnDate,
                booking.Status,
                Purpose = booking.Purpose
            });
        }

        // Booking history & tracking for admin
        [Authorize(Roles = "Admin")]
        [HttpGet("admin/history")]
        public async Task<IActionResult> GetBookingHistory(
            string? search,
            string? status,
            DateTime? startDate,
            DateTime? endDate,
            int? roomId,
            string? sortOrder)
        {
            var query = _context.Bookings
                .Include(b => b.User)
                .Include(b => b.Room)
                .AsQueryable();

            if (!string.IsNullOrEmpty(search))
            {
                query = query.Where(b =>
                    (b.User != null && b.User.Username.Contains(search)) ||
                    (b.Room != null && b.Room.Name.Contains(search))
                );
            }

            if (!string.IsNullOrEmpty(status))
            {
                query = query.Where(b => b.Status == status);
            }

            if (startDate.HasValue)
            {
                query = query.Where(b => b.BorrowDate >= startDate.Value);
            }

            if (endDate.HasValue)
            {
                query = query.Where(b => b.BorrowDate <= endDate.Value);
            }

            if (roomId.HasValue)
            {
                query = query.Where(b => b.RoomId == roomId.Value);
            }

            var order = sortOrder?.ToLower() ?? "desc";
            query = order == "asc"
                ? query.OrderBy(b => b.BorrowDate)
                : query.OrderByDescending(b => b.BorrowDate);

            var bookings = await _context.Bookings
            .Include(b => b.User)
            .Include(b => b.Room)
            .Where(b => b.Status == "Approved" || b.Status == "Rejected") // filter langsung di query
            .Select(b => new
            {
                b.Id,
                b.UserId,
                UserName = b.User != null ? b.User.Username : "Unknown User",
                b.RoomId,
                RoomName = b.Room != null ? b.Room.Name : "Unknown Room",
                RoomLocation = b.Room != null ? b.Room.Location : "-",
                RoomCapacity = b.Room != null ? b.Room.Capacity : 0,
                b.BorrowDate,
                b.ReturnDate,
                b.Status,
                Purpose = b.Purpose
            })
            .ToListAsync();


            return Ok(bookings);
        }
    }
}
