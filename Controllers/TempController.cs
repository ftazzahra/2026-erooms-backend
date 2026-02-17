using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class TempController : ControllerBase
{
    [HttpGet("hash")]
    public IActionResult GetHash()
    {
        var adminHash = BCrypt.Net.BCrypt.HashPassword("admin123");
        var mahasiswaHash = BCrypt.Net.BCrypt.HashPassword("12345");

        return Ok(new
        {
            admin = adminHash,
            mahasiswa = mahasiswaHash
        });
    }
}
