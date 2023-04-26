using Artisan.CommonComponents.JsInterop;
using Microsoft.JSInterop;

namespace Artisan.Pages.CharGen.Interop;

public class HighlightJsInterop : JsInteropBase
{
    public HighlightJsInterop(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }

    protected override string JsFileRelativePath { get; } = "Artisan.Pages.CharGen/highlight.min.js";

    public async ValueTask UpdateHighlight()
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("hljs");
    }
}