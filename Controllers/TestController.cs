using Microsoft.AspNetCore.Mvc;

namespace erooms.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public string Get()
    {
        return "API erooms jalan";
    }
}
