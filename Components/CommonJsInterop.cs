using Microsoft.JSInterop;

namespace Artisan.CommonComponents;

// This class provides an example of how JavaScript functionality can be wrapped
// in a .NET class for easy consumption. The associated JavaScript module is
// loaded on demand when first needed.
//
// This class can be registered as scoped DI service and then injected into Blazor
// components for use.

public class CommonJsInterop : IAsyncDisposable
{
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;

    public CommonJsInterop(IJSRuntime jsRuntime)
    {
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => 
            jsRuntime.InvokeAsync<IJSObjectReference>("import", "./_content/Artisan.CommonComponents/module.js")
            .AsTask());
    }

    public async ValueTask AlertAsync(string message)
    {
        var module = await _moduleTask.Value;
        await module.InvokeVoidAsync("callAlert", message);
    }
    
    public async ValueTask<string> PromptAsync(string message)
    {
        var module = await _moduleTask.Value;
        return await module.InvokeAsync<string>("showPrompt", message);
    }

    public async ValueTask DownloadFileAsync(Stream content, string fileName)
    {
        var module = await _moduleTask.Value;
        DotNetStreamReference streamRef = new(content);

        await module.InvokeVoidAsync("downloadFile", fileName, streamRef);
    }

    public async ValueTask DisposeAsync()
    {
        if (_moduleTask.IsValueCreated)
        {
            var module = await _moduleTask.Value;
            await module.DisposeAsync();
        }
    }
}