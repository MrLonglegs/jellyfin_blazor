using jellyfin_blazor;
using jellyfin_blazor.Identity;
using jellyfin_blazor.Services;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped<ILocalStorage, LocalStorageProvider>();
builder.Services.AddScoped<AuthenticationContext>();
builder.Services.AddScoped<HttpService>();
await builder.Build().RunAsync();
