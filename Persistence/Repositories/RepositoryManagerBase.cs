using Application.IRepositories;

namespace Persistence.Repositories;

public class RepositoryManagerBase
{
    private readonly Lazy<IPurchaseRepo> _lazyPurchaseRepo;
}