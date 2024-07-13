namespace Application.IRepositories;

public interface IRepositoryManager
{
    ICustomerRepo CustomerRepo { get; }
    ISupplierRepo SupplierRepo { get; }
    IProductCategoryRepo ProductCategoryRepo { get; }
    IProductRepo ProductRepo { get; }
    IPurchaseRepo PurchaseRepo { get; }
    IUnitOfWorkRepo UnitOfWorkRepo { get; }
    IPurchaseDetailRepo PurchaseDetailRepo { get; }
}
