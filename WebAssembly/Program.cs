using Extensions;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using WebAssembly;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
var configuration = builder.Configuration;
var hostEnvironment = builder.HostEnvironment;

builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.ConfigureServices(configuration, hostEnvironment);

await builder.Build().RunAsync();
