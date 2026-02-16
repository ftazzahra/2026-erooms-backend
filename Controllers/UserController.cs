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
    [Authorize]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/users/me
        [HttpGet("me")]
        public async Task<ActionResult<UserResponseDto>> GetProfile()
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            return new UserResponseDto
            {
                Id = user.Id,
                Username = user.Username,
                Role = user.Role
            };
        }

        // PUT: api/users/update
        [HttpPut("update")]
        public async Task<IActionResult> UpdateProfile(UpdateUserDto dto)
        {
            var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

            var user = await _context.Users.FindAsync(userId);

            if (user == null)
                return NotFound();

            // Update username jika diisi
            if (!string.IsNullOrWhiteSpace(dto.Username))
                user.Username = dto.Username;

            // Update password jika diisi
            if (!string.IsNullOrWhiteSpace(dto.Password))
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(dto.Password);

            await _context.SaveChangesAsync();

            return Ok(new { message = "Profile updated successfully" });
        }
    }
}