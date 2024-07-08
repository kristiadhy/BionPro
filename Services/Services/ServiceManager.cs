using Application.IRepositories;
using AutoMapper;
using Domain.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Configuration;
using Serilog;
using Services.Contracts;
using Services.Contracts.IServices;
using Services.Services;

namespace Services;

public sealed class ServiceManager : IServiceManager
{
    private readonly Lazy<ICustomerService> _lazyCustomerService;
    private readonly Lazy<ISupplierService> _lazySupplierService;
    private readonly Lazy<IProductCategoryService> _lazyProductCategoryService;
    private readonly Lazy<IProductService> _lazyProductService;
    private readonly Lazy<IAuthenticationService> _lazyAuthenticationService;
    private readonly Lazy<IPurchaseService> _lazyPurchaseService;

    public ServiceManager(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger, UserManager<UserModel> userManager, IConfiguration configuration)
    {
        _lazyCustomerService = new Lazy<ICustomerService>(() => new CustomerService(repositoryManager, mapper, logger));
        _lazySupplierService = new Lazy<ISupplierService>(() => new SupplierService(repositoryManager, mapper, logger));
        _lazyProductCategoryService = new Lazy<IProductCategoryService>(() => new ProductCategoryService(repositoryManager, mapper, logger));
        _lazyProductService = new Lazy<IProductService>(() => new ProductService(repositoryManager, mapper, logger));
        _lazyAuthenticationService = new Lazy<IAuthenticationService>(() => new AuthenticationService(repositoryManager, mapper, logger, userManager, configuration));
        _lazyPurchaseService = new Lazy<IPurchaseService>(() => new PurchaseService(repositoryManager, mapper, logger));
    }

    public ICustomerService CustomerService => _lazyCustomerService.Value;
    public ISupplierService SupplierService => _lazySupplierService.Value;
    public IProductCategoryService ProductCategoryService => _lazyProductCategoryService.Value;
    public IProductService ProductService => _lazyProductService.Value;
    public IAuthenticationService AuthenticationService => _lazyAuthenticationService.Value;
    public IPurchaseService PurchaseService => _lazyPurchaseService.Value;
}
