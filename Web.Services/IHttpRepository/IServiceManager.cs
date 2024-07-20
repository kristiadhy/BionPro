namespace Web.Services.IHttpRepository;

public interface IServiceManager
{
    ICustomerService CustomerService { get; }
    ISupplierService SupplierService { get; }
    IProductCategoryService ProductCategoryService { get; }
    IProductService ProductService { get; }
    IPurchaseService PurchaseService { get; }
    IPurchaseDetailService PurchaseDetailService { get; }
    ISaleService SaleService { get; }
    ISaleDetailService SaleDetailService { get; }
}
