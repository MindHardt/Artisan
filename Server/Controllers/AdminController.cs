using Artisan.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Artisan.Server.Controllers;

[Authorize]
[ApiController]
[Route("api/[controller]")]
public class AdminController : ControllerBase
{

    [HttpPost("/isalive")]
    public IActionResult IsAlive([FromServices] ApplicationDbContext dbContext)
    {
        return Ok();
    }
}