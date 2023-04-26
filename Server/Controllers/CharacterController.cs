using Artisan.Data.Repos;
using Microsoft.AspNetCore.Mvc;

namespace Artisan.Server.Controllers;

[ApiController]
[Route("[controller]")]
public class CharacterController : ControllerBase
{
    private readonly CharacterRepository _characterRepository;

    public CharacterController(CharacterRepository characterRepository)
    {
        _characterRepository = characterRepository;
    }

    [HttpGet("/overview")]
    public async Task<IActionResult> GetOverviews(
        [FromQuery] int page = 0)
    {
        var overviews = await _characterRepository.GetCharacterOverviews(page);

        return Ok(overviews);
    }
}