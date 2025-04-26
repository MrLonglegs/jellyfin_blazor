using jellyfin_blazor;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient
{
    BaseAddress = new Uri("https://jellyfin.brueffer24.de"),
    DefaultRequestHeaders =
    {
        { "User-Agent", "Jellyfin Blazor WebClient v0.0.1" },
        { "Accept", "application/json" },
    }
});

await builder.Build().RunAsync();
