using Artisan.CommonComponents.JsInterop;
using Microsoft.JSInterop;

namespace Artisan.Pages.DiceThrower;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class DiceJsInterop : JsInteropBase
{
    protected override string JsFileRelativePath => "Artisan.Pages.DiceThrower/diceModule.js";

    public DiceJsInterop(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }

    public async ValueTask AlertAsync(string message)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("showAlert", message);
    }

    public async ValueTask<bool> ConfirmAsync(string message)
    {
        var module = await GetModuleAsync();
        return await module.InvokeAsync<bool>("requestConfirm", message);
    }
}
