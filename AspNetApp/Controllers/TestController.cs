using Microsoft.AspNetCore.Mvc;

namespace AspNetApp.Controllers;

[ApiController]
[Route("[controller]")]
public class TestController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {
        return Ok(new { message = "API is working!" });
    }
} 