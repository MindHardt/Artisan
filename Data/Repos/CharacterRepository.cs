using ArkLens.Snapshots;
using Artisan.Data.Models;
using Microsoft.EntityFrameworkCore;

namespace Artisan.Data.Repos;

public class CharacterRepository : RepositoryBase<CharacterSnapshot>
{
    protected const int CharactersPerPage = 10;
    
    public CharacterRepository(DbContext context) : base(context)
    {
    }

    public async ValueTask<ICollection<CharacterOverview>> GetCharacterOverviews(int page = 0)
    {
        var rawData = await Set
            .OrderBy(c => c.Name)
            .Skip(page * CharactersPerPage)
            .Take(CharactersPerPage)
            .Select(cs => new
            {
                Name = cs.Name,
                Race = cs.Race
            })
            .ToArrayAsync();

        var result = rawData
            .Select(d => new CharacterOverview()
            {
                Name = d.Name ?? string.Empty,
                Race = d.Race ?? string.Empty,
            })
            .ToArray();

        return result;
    }

    public ValueTask<CharacterSnapshot?> GetCharacter(string name) => Set
        .FindAsync(name);

}