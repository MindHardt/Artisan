using ArkLens.Snapshots;
using Microsoft.AspNetCore.Identity;

namespace Artisan.Data.Models;

public class ApplicationUser : IdentityUser
{
    public ICollection<CharacterSnapshot> CharacterSnapshots { get; set; } = new List<CharacterSnapshot>();
}
