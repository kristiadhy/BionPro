using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Radzen;
using Services;
using Web.Services.Features;
using WebAssembly.Extensions;

namespace Extensions;

public static class Startup
{
    public static IServiceCollection ConfigureServices(this IServiceCollection services, WebAssemblyHostConfiguration configuration, IWebAssemblyHostEnvironment hostEnvironment)
    {
        //Register web host environment record. We need to do it this way because we can't use IWebAssemblyHostEnvironment in the class library.
        services.AddSingleton(sp => new WebHostEnvironment(hostEnvironment.IsDevelopment(), hostEnvironment.IsProduction()));

        //Register HTTP service project
        services.ConfigureHTTPServices(configuration);

        services.AddRadzenComponents();
        services.AddCascadingAuthenticationState();

        // Configure the appsettings.json to be read
        configuration.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true);

        //Register the local services
        services.Configure<ApplicationDetail>(configuration.GetSection("ApplicationDetail"));
        services.ConfigureCustomComponentServices();
        services.ConfigureStateManagementServices();

        return services;
    }
}
