using Microsoft.JSInterop;

namespace Artisan.CommonComponents.JsInterop;

public abstract class JsInteropBase : IAsyncDisposable
{
    /// <summary>
    /// The relative path to the .js file. It
    /// skips the <code>"./_content/"</code> part.
    /// </summary>
    protected abstract string JsFileRelativePath { get; }
    
    /// <summary>
    /// Full path to .js file.
    /// </summary>
    private string JsFileFullPath => "./_content/" + JsFileRelativePath;
    
    /// <summary>
    /// The underlying <see cref="IJSRuntime"/> object, wrapped for lazy evaluation.
    /// </summary>
    private readonly Lazy<Task<IJSObjectReference>> _moduleTask;
    
    /// <summary>
    /// Gets the javascript module.
    /// </summary>
    /// <returns></returns>
    protected Task<IJSObjectReference> GetModuleAsync() => _moduleTask.Value;

    protected JsInteropBase(IJSRuntime jsRuntime)
    {
        _moduleTask = new Lazy<Task<IJSObjectReference>>(() => 
            jsRuntime.InvokeAsync<IJSObjectReference>("import", JsFileFullPath).AsTask());
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