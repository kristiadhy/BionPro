using Blazored.LocalStorage;
using Microsoft.AspNetCore.Components.Authorization;
using Newtonsoft.Json;
using Services.Extensions;
using Web.Services.IHttpRepository;

namespace Web.Services.HttpRepository;

public sealed class ServiceManager : IServiceManager
{
  private readonly Lazy<ICustomerHttpService> _lazyCustomerService;
  private readonly Lazy<ISupplierHttpService> _lazySupplierService;
  private readonly Lazy<IProductCategoryHttpService> _lazyProductCategoryService;
  private readonly Lazy<IProductHttpService> _lazyProductService;
  private readonly Lazy<IPurchaseHttpService> _lazyPurchaseService;
  private readonly Lazy<IPurchaseDetailHttpService> _lazyPurchaseDetailService;
  private readonly Lazy<ISaleHttpService> _lazySaleService;
  private readonly Lazy<ISaleDetailHttpService> _lazySaleDetailService;

  public ServiceManager(CustomHttpClient apiService, JsonSerializerSettings settings, AuthenticationStateProvider authStateProvider, ILocalStorageService localStorage)
  {
    _lazyCustomerService = new Lazy<ICustomerHttpService>(() => new CustomerHttpService(apiService, settings));
    _lazySupplierService = new Lazy<ISupplierHttpService>(() => new SupplierHttpService(apiService, settings));
    _lazyProductCategoryService = new Lazy<IProductCategoryHttpService>(() => new ProductCategoryHttpService(apiService, settings));
    _lazyProductService = new Lazy<IProductHttpService>(() => new ProductHttpService(apiService, settings));
    _lazyPurchaseService = new Lazy<IPurchaseHttpService>(() => new PurchaseHttpService(apiService, settings));
    _lazyPurchaseDetailService = new Lazy<IPurchaseDetailHttpService>(() => new PurchaseDetailHttpService(apiService, settings));
    _lazySaleService = new Lazy<ISaleHttpService>(() => new SaleHttpService(apiService, settings));
    _lazySaleDetailService = new Lazy<ISaleDetailHttpService>(() => new SaleDetailHttpService(apiService, settings));
  }

  public ICustomerHttpService CustomerService => _lazyCustomerService.Value;
  public ISupplierHttpService SupplierService => _lazySupplierService.Value;
  public IProductCategoryHttpService ProductCategoryService => _lazyProductCategoryService.Value;
  public IProductHttpService ProductService => _lazyProductService.Value;
  public IPurchaseHttpService PurchaseService => _lazyPurchaseService.Value;
  public IPurchaseDetailHttpService PurchaseDetailService => _lazyPurchaseDetailService.Value;
  public ISaleHttpService SaleService => _lazySaleService.Value;
  public ISaleDetailHttpService SaleDetailService => _lazySaleDetailService.Value;
}
