namespace Application.IRepositories;

public interface IRepositoryManager
{
    ICustomerRepo CustomerRepo { get; }
    ISupplierRepo SupplierRepo { get; }
    IProductCategoryRepo ProductCategoryRepo { get; }
    IUnitOfWorkRepo UnitOfWorkRepo { get; }
}
