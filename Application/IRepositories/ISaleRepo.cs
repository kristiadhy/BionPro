using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface ISaleRepo : IRepositoryBase<SaleModel>
{
  Task<PagedList<SaleDtoForSummary>> GetSummaryByParametersAsync(SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default);
  //Task<PagedList<SaleModel>> GetByParametersAsync(SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default);
  Task<SaleModel?> GetByIDAsync(int saleID, bool trackChanges, CancellationToken cancellationToken = default);
  Task<bool> CheckTransactionCodeExistAsync(string transactionCode, bool trackChanges, CancellationToken cancellationToken = default);
}
