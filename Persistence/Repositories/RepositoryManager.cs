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
    private readonly Lazy<IPurchaseRepo> _lazyPurchaseRepo;
    private readonly Lazy<IPurchaseDetailRepo> _lazyPurchaseDetailRepo;
    private readonly Lazy<ISaleRepo> _lazySaleRepo;
    private readonly Lazy<ISaleDetailRepo> _lazySaleDetailRepo;

    public RepositoryManager(AppDBContext dbContext)
    {
        _lazyCustomerRepo = new Lazy<ICustomerRepo>(() => new CustomerRepo(dbContext));
        _lazyUnitOfWorkRepo = new Lazy<IUnitOfWorkRepo>(() => new UnitOfWorkRepo(dbContext));
        _lazySupplierRepo = new Lazy<ISupplierRepo>(() => new SupplierRepo(dbContext));
        _lazyProductCategoryRepo = new Lazy<IProductCategoryRepo>(() => new ProductCategoryRepo(dbContext));
        _lazyProductRepo = new Lazy<IProductRepo>(() => new ProductRepo(dbContext));
        _lazyPurchaseRepo = new Lazy<IPurchaseRepo>(() => new PurchaseRepo(dbContext));
        _lazyPurchaseDetailRepo = new Lazy<IPurchaseDetailRepo>(() => new PurchaseDetailRepo(dbContext));
        _lazySaleRepo = new Lazy<ISaleRepo>(() => new SaleRepo(dbContext));
        _lazySaleDetailRepo = new Lazy<ISaleDetailRepo>(() => new SaleDetailRepo(dbContext));
    }

    public ICustomerRepo CustomerRepo => _lazyCustomerRepo.Value;
    public IUnitOfWorkRepo UnitOfWorkRepo => _lazyUnitOfWorkRepo.Value;
    public ISupplierRepo SupplierRepo => _lazySupplierRepo.Value;
    public IProductCategoryRepo ProductCategoryRepo => _lazyProductCategoryRepo.Value;
    public IProductRepo ProductRepo => _lazyProductRepo.Value;
    public IPurchaseRepo PurchaseRepo => _lazyPurchaseRepo.Value;
    public IPurchaseDetailRepo PurchaseDetailRepo => _lazyPurchaseDetailRepo.Value;
    public ISaleRepo SaleRepo => _lazySaleRepo.Value;
    public ISaleDetailRepo SaleDetailRepo => _lazySaleDetailRepo.Value;
}
