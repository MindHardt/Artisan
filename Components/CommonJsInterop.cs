using Artisan.CommonComponents.JsInterop;
using Microsoft.JSInterop;

namespace Artisan.CommonComponents;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class CommonJsInterop : JsInteropBase
{
    protected override string JsFileRelativePath { get; } = "Artisan.CommonComponents/module.js";
    
    public CommonJsInterop(IJSRuntime jsRuntime) : base(jsRuntime)
    {
    }

    public async ValueTask AlertAsync(string message)
    {
        var module = await GetModuleAsync();
        await module.InvokeVoidAsync("callAlert", message);
    }
    
    public async ValueTask<string> PromptAsync(string message)
    {
        var module = await GetModuleAsync();
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    public async ValueTask DownloadFileAsync(Stream content, string fileName)
    {
        var module = await GetModuleAsync();
        DotNetStreamReference streamRef = new(content);

        await module.InvokeVoidAsync("downloadFile", fileName, streamRef);
    }
}