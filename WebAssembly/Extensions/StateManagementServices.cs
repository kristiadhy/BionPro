namespace WebAssembly.Extensions;

public static class StateManagementServices
{
    public static IServiceCollection ConfigureStateManagementServices(this IServiceCollection services)
    {
        //This is why we use AddScope and not AddSingleton here.
        // Scoped: Service state is specific to a particular user session or a set of user interactions. It's maintained while the user stays on the same page or navigates within the same scope.
        // Singleton: Service state is shared across all users and sessions for the lifetime of the application.All users will see the same data.
        //When you need to store state for a specific user session (using scoped services) or share state globally across all users (using singleton services).
        services.AddScoped<CustomerDisplayState>();
        services.AddScoped<CustomerDisplayFilterState>();
        services.AddScoped<CustomerInputState>();
        services.AddScoped<CustomerDropdownState>();

        services.AddScoped<ProductCategoryDisplayState>();
        services.AddScoped<ProductCategoryDropdownState>();
        services.AddScoped<ProductCategoryInputState>();

        services.AddScoped<ProductDisplayState>();
        services.AddScoped<ProductDisplayFilterState>();
        services.AddScoped<ProductInputState>();
        services.AddScoped<ProductDropdownState>();

        services.AddScoped<PurchaseDisplayState>();
        services.AddScoped<PurchaseDisplayFilterState>();
        services.AddScoped<PurchaseDetailState>();
        services.AddScoped<PurchaseInputState>();

        services.AddScoped<SaleDisplayState>();
        services.AddScoped<SaleDisplayFilterState>();
        services.AddScoped<SaleDetailState>();
        services.AddScoped<SaleInputState>();

        services.AddScoped<SupplierDisplayState>();
        services.AddScoped<SupplierDisplayFilterState>();
        services.AddScoped<SupplierInputState>();
        services.AddScoped<SupplierDropdownState>();

        services.AddScoped<UserRegistrationState>();

        return services;
    }
}
