using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface ISaleDetailRepo
{
    Task<PagedList<SaleDetailModel>> GetByIDAsync(int saleID, SaleDetailParam saleDetailParam, bool trackChanges, CancellationToken cancellationToken = default);
}
