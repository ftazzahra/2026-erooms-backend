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

        // ===============================
        // 1️⃣ CREATE BOOKING (USER)
        // ===============================
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
                Status = "Pending"
            };

            _context.Bookings.Add(booking);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetBookingDetail), 
                new { id = booking.Id }, 
                new { message = "Booking created successfully", bookingId = booking.Id });
        }

        // ===============================
        // 2️⃣ GET MY BOOKINGS
        // ===============================
        [Authorize(Roles = "User")]
        [HttpGet("my")]
        public async Task<IActionResult> GetMyBookings()
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var bookings = await _context.Bookings
                .Where(b => b.UserId == userId)
                .Include(b => b.Room)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate,
                    Status = b.Status
                })
                .ToListAsync();

            return Ok(bookings);
        }

        // ===============================
        // 3️⃣ GET DETAIL BOOKING
        // ===============================
        [Authorize(Roles = "User")]
        [HttpGet("{id}")]
        public async Task<IActionResult> GetBookingDetail(int id)
        {
            var userId = int.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);

            var booking = await _context.Bookings
                .Where(b => b.Id == id && b.UserId == userId)
                .Include(b => b.Room)
                .Select(b => new BookingResponseDto
                {
                    Id = b.Id,
                    RoomId = b.RoomId,
                    RoomName = b.Room.Name,
                    BorrowDate = b.BorrowDate,
                    ReturnDate = b.ReturnDate,
                    Status = b.Status
                })
                .FirstOrDefaultAsync();

            if (booking == null)
                return NotFound(new { message = "Booking not found" });

            return Ok(booking);
        }

        // ===============================
        // 4️⃣ UPDATE BOOKING
        // ===============================
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

            await _context.SaveChangesAsync();

            return Ok(new { message = "Booking updated successfully" });
        }

        // ===============================
        // 5️⃣ DELETE BOOKING
        // ===============================
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

        // ===============================
        // ADMIN - GET ALL BOOKINGS
        // ===============================
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
                    b.BorrowDate,
                    b.ReturnDate,
                    b.Status
                })
                .ToListAsync();

            return Ok(bookings);
        }


        // ===============================
        // UPDATE STATUS BOOKING (ADMIN)
        // ===============================
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
                booking.Status
            });
        }
    }
}