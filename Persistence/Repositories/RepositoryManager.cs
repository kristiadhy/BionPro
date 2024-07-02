using Application.IRepositories;
using Persistence.Context;

namespace Persistence.Repositories;

public sealed class RepositoryManager : IRepositoryManager
{
    private readonly Lazy<IUnitOfWorkRepo> _lazyUnitOfWorkRepo;
    private readonly Lazy<ICustomerRepo> _lazyCustomerRepo;
    private readonly Lazy<ISupplierRepo> _lazySupplierRepo;
    private readonly Lazy<IProductCategoryRepo> _lazyProductCategoryRepo;
    private readonly Lazy<IProductRepo> _lazyProductRepo;

    public RepositoryManager(AppDBContext dbContext)
    {
        _lazyCustomerRepo = new Lazy<ICustomerRepo>(() => new CustomerRepo(dbContext));
        _lazyUnitOfWorkRepo = new Lazy<IUnitOfWorkRepo>(() => new UnitOfWorkRepo(dbContext));
        _lazySupplierRepo = new Lazy<ISupplierRepo>(() => new SupplierRepo(dbContext));
        _lazyProductCategoryRepo = new Lazy<IProductCategoryRepo>(() => new ProductCategoryRepo(dbContext));
        _lazyProductRepo = new Lazy<IProductRepo>(() => new ProductRepo(dbContext));
    }

    public ICustomerRepo CustomerRepo => _lazyCustomerRepo.Value;
    public IUnitOfWorkRepo UnitOfWorkRepo => _lazyUnitOfWorkRepo.Value;
    public ISupplierRepo SupplierRepo => _lazySupplierRepo.Value;
    public IProductCategoryRepo ProductCategoryRepo => _lazyProductCategoryRepo.Value;
    public IProductRepo ProductRepo => _lazyProductRepo.Value;
}
