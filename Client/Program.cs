using System.Text.Json;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Authentication;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Artisan.Client;
using Artisan.CommonComponents;
using Artisan.CommonComponents.JsInterop;
using Artisan.Pages.DiceThrower;
using Blazored.LocalStorage;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddHttpClient("Artisan.ServerAPI", client => client.BaseAddress = new Uri(builder.HostEnvironment.BaseAddress))
    .AddHttpMessageHandler<BaseAddressAuthorizationMessageHandler>();

// Supply HttpClient instances that include access tokens when making requests to the server project
builder.Services.AddScoped(sp => sp.GetRequiredService<IHttpClientFactory>().CreateClient("Artisan.ServerAPI"));
builder.Services.AddJsInterops();
// builder.Services.AddScoped<CommonJsInterop>();
// builder.Services.AddScoped<DiceJsInterop>();
builder.Services.AddTransient(_ => new JsonSerializerOptions
{
    WriteIndented = true,
    Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
});

builder.Services.AddBlazoredLocalStorage();

builder.Services.AddApiAuthorization();

await builder.Build().RunAsync();
