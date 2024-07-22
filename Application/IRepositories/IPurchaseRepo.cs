using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using System.Linq.Expressions;

namespace Application.IRepositories;
public interface IPurchaseRepo : IRepositoryBase<PurchaseModel>
{
    Task<PagedList<PurchaseDtoForSummary>> GetSummaryByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default);
    //Task<PagedList<PurchaseModel>> GetByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PurchaseModel?> GetByIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PurchaseModel?> GetByConditionAsync(Expression<Func<PurchaseModel, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
    Task<bool> CheckTransactionCodeExistAsync(string transactionCode, bool trackChanges, CancellationToken cancellationToken = default);
    void AttachEntity(PurchaseModel entity);
}
