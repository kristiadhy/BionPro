using Domain.DTO;
using Domain.Parameters;

namespace Services.Contracts.IServices;
public interface IPurchaseDetailService
{
  Task<(IEnumerable<PurchaseDetailDto> purchaseDetailDto, MetaData metaData)> GetByPurchaseDetailByIDAsync(int purchaseID, PurchaseDetailParam purchaseDetailParam, bool trackChanges, CancellationToken cancellationToken = default);
}
