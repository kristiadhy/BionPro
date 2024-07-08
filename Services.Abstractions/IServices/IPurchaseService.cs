using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts.IServices;
public interface IPurchaseService : IServiceBase<PurchaseDto>
{
    Task<(IEnumerable<PurchaseDtoForQueries> purchaseDto, MetaData metaData)> GetByParametersAsync(int purchaseID, PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PurchaseDto> GetByPurchaseIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default);
    Task<(PurchaseDto purchaseToPatch, PurchaseModel purchase)> GetPurchaseForPatchAsync(int purchaseID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(PurchaseDto purchaseDto, PurchaseModel purchaseModel, CancellationToken cancellationToken = default);
}
