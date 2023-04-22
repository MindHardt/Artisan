using Arklens.Builders;
using Microsoft.AspNetCore.Components;

namespace Artisan.Pages.CharGen.Components;

public class CharacterBuilderComponent : ComponentBase
{
    [Parameter] 
    public CharacterBuilder Builder { get; set; } = null!;
}