namespace WebAssembly.Extensions;

public static class CustomComponentServices
{
    public static IServiceCollection ConfigureCustomComponentServices(this IServiceCollection services)
    {
        services.AddScoped<CustomModalService>();
        services.AddScoped<CustomNotificationService>();
        services.AddScoped<CustomTooltipService>();

        return services;
    }
}
