using Asp.Versioning;
using Microsoft.AspNetCore.Mvc;

namespace ThreadPilot.Vehicle.Controllers;

[ApiController]
[Route("[controller]")]
[ApiVersion("1.0")]
public class HealthController : ControllerBase
{
    [HttpGet]
    public IActionResult Get()
    {         
        return Ok("Insurance API is healthy.");
    }
}
