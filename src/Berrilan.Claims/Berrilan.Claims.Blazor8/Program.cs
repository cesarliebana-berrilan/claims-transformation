using Berrilan.Claims.Blazor8;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddConfigurations(builder.Configuration);
builder.Services.AddServices(builder.Configuration);
builder.Services.AddSecurityLayer(builder.Configuration);

await builder.Build().RunAsync();
