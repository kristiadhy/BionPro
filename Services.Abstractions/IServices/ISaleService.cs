using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts.IServices;
public interface ISaleService : IServiceBase<SaleDto>
{
    Task<(IEnumerable<SaleDtoForSummary> saleDto, MetaData metaData)> GetSummaryByParametersAsync(int saleID, SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default);
    //Task<(IEnumerable<SaleDto> saleDto, MetaData metaData)> GetByParametersAsync(int saleID, SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<SaleDto> GetBySaleIDAsync(int saleID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(int saleID, bool trackChanges, CancellationToken cancellationToken = default);
    Task<(SaleDto saleToPatch, SaleModel sale)> GetSaleForPatchAsync(int saleID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(SaleDto saleDto, SaleModel saleModel, CancellationToken cancellationToken = default);
}
