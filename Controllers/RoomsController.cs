using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using erooms.Data;
using erooms.Models;
using erooms.DTOs;

namespace erooms.Controllers
{
    //update authorization
    [Route("api/[controller]")]
    [ApiController]
    public class RoomsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RoomsController(AppDbContext context)
        {
            _context = context;
        }

        // Get
        [Authorize]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Room>>> GetRooms()
        {
            return await _context.Rooms.ToListAsync();
        }

        // Get By Id
        [HttpGet("{id}")]
        public async Task<ActionResult<Room>> GetRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound();

            return room;
        }

        // Post    
        [Authorize(Roles = "Admin")]
        [HttpPost]
        public async Task<ActionResult<Room>> PostRoom(RoomRequestDto dto)
        {
            var room = new Room
            {
                Name = dto.Name,
                Location = dto.Location,          
                Capacity = dto.Capacity,
                IsAvailable = dto.IsAvailable     
            };

            _context.Rooms.Add(room);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetRoom), new { id = room.Id }, room);
        }

        // Put By Id
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public async Task<IActionResult> UpdateRoom(int id, RoomRequestDto dto)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound();

            room.Name = dto.Name;
            room.Location = dto.Location;
            room.Capacity = dto.Capacity;
            room.IsAvailable = dto.IsAvailable;

            await _context.SaveChangesAsync();

            return NoContent();
        }


        // updated Delete By Id
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            var room = await _context.Rooms.FindAsync(id);

            if (room == null)
                return NotFound();

            _context.Rooms.Remove(room);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
