using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _lazyCustomerService;
    private readonly Lazy<ISupplierService> _lazySupplierService;
    private readonly Lazy<IProductCategoryService> _lazyProductCategoryService;
    private readonly Lazy<IProductService> _lazyProductService;
    private readonly Lazy<IPurchaseService> _lazyPurchaseService;
    private readonly Lazy<IPurchaseDetailService> _lazyPurchaseDetailService;

    public ServiceManager(CustomHttpClient apiService, JsonSerializerSettings settings, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
    {
        _lazyCustomerService = new Lazy<ICustomerService>(() => new CustomerService(apiService, settings));
        _lazySupplierService = new Lazy<ISupplierService>(() => new SupplierService(apiService, settings));
        _lazyProductCategoryService = new Lazy<IProductCategoryService>(() => new ProductCategoryService(apiService, settings));
        _lazyProductService = new Lazy<IProductService>(() => new ProductService(apiService, settings));
        _lazyPurchaseService = new Lazy<IPurchaseService>(() => new PurchaseService(apiService, settings));
        _lazyPurchaseDetailService = new Lazy<IPurchaseDetailService>(() => new PurchaseDetailService(apiService, settings));
    }

    public ICustomerService CustomerService => _lazyCustomerService.Value;
    public ISupplierService SupplierService => _lazySupplierService.Value;
    public IProductCategoryService ProductCategoryService => _lazyProductCategoryService.Value;
    public IProductService ProductService => _lazyProductService.Value;
    public IPurchaseService PurchaseService => _lazyPurchaseService.Value;
    public IPurchaseDetailService PurchaseDetailService => _lazyPurchaseDetailService.Value;
}
