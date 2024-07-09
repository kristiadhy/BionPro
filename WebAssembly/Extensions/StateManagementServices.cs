using WebAssembly.StateManagement;

namespace WebAssembly.Extensions;

public static class StateManagementServices
{
    public static IServiceCollection ConfigureStateManagementServices(this IServiceCollection services)
    {
        services.AddScoped<CustomerState>();
        services.AddScoped<SupplierState>();
        services.AddScoped<ProductCategoryState>();
        services.AddScoped<ProductState>();
        services.AddScoped<PurchaseState>();
        services.AddScoped<PurchaseDetailState>();

        return services;
    }
}
