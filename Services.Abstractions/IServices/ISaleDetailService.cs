using Domain.DTO;
using Domain.Parameters;

namespace Services.Contracts.IServices;
public interface ISaleDetailService
{
    Task<(IEnumerable<SaleDetailDto> saleDetailDto, MetaData metaData)> GetBySaleDetailByIDAsync(int saleID, SaleDetailParam saleDetailParam, bool trackChanges, CancellationToken cancellationToken = default);
}
