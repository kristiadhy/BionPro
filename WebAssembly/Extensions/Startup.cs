using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.Options;
using Radzen;
using Services;
using Web.Services.Features;
using WebAssembly.Extensions;

namespace Extensions;

//The Options Pattern (1)
//https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0
//An alternative approach when using the options pattern is to bind the Position section and add it to the dependency injection service container.

//Root-level cascading values (2)
//Root-level cascading values can be registered for the entire component hierarchy.Named cascading values and subscriptions for update notifications are supported.

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

        //This is also injected into DI container, you can use it in your components by using IOptions<ApplicationDetail> (1)
        services.Configure<ApplicationDetail>(configuration.GetSection("ApplicationDetail"));
        //Look at the difference between IOption and IOptionsMonitor here:
        //https://alirezafarokhi.medium.com/difference-between-ioptions-ioptionssnapshot-and-ioptionsmonitor-in-asp-netcore-587954bbcea
        //https://learn.microsoft.com/en-us/aspnet/core/fundamentals/configuration/options?view=aspnetcore-8.0
        //When you use IOptionsMonitor, it works as a Singleton and like IOptions can be injected in any service lifetime.this interface supports reloading the changed configurations after app has started
        services.AddCascadingValue(sp => sp.GetRequiredService<IOptionsMonitor<ApplicationDetail>>().CurrentValue); //(1 & 2)

        //Register the local services
        services.ConfigureCustomComponentServices();
        services.ConfigureStateManagementServices();

        return services;
    }
}
