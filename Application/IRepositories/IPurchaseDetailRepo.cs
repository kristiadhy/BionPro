using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface IPurchaseDetailRepo
{
    Task<PagedList<PurchaseDetailModel>> GetByIDAsync(int purchaseID, PurchaseDetailParam purchaseDetailParam, bool trackChanges, CancellationToken cancellationToken = default);
}
