namespace Web.Services.IHttpRepository;

public interface IServiceManager
{
    ICustomerHttpService CustomerService { get; }
    ISupplierHttpService SupplierService { get; }
    IProductCategoryHttpService ProductCategoryService { get; }
    IProductHttpService ProductService { get; }
    IPurchaseHttpService PurchaseService { get; }
    IPurchaseDetailHttpService PurchaseDetailService { get; }
    ISaleHttpService SaleService { get; }
    ISaleDetailHttpService SaleDetailService { get; }
}
