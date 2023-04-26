namespace Artisan.Data.Models;

public class CharacterOverview
{
    public required string Name { get; set; }
    public required string Race { get; set; }

    public override string ToString() =>
        $"{Name} : {Race}";
}