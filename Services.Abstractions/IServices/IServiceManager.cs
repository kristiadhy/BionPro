
using Services.Contracts.IServices;

namespace Services.Contracts;

public interface IServiceManager
{
    ICustomerService CustomerService { get; }
    ISupplierService SupplierService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IAuthenticationService AuthenticationService { get; }
}
