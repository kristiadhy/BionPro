using Web.Services.HttpRepository;

namespace Web.Services.IHttpRepository;

public interface IServiceManager
{
    ICustomerService CustomerService { get; }
    ISupplierService SupplierService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IAuthenticationService AuthService { get; }
    RefreshTokenService RefreshTokenService { get; }
    HttpInterceptorService InterceptorService { get; }
}
