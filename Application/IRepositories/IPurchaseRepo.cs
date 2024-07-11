using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface IPurchaseRepo : IRepositoryBase<PurchaseModel>
{
    Task<PagedList<PurchaseDtoForQueries>> GetByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PurchaseModel?> GetByIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default);
    Task<bool> CheckTransactionCodeExistAsync(string transactionCode, bool trackChanges, CancellationToken cancellationToken = default);
}
