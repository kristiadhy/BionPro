using Domain.Entities;
using Domain.Parameters;
using System.Linq.Expressions;

namespace Application.IRepositories;
public interface ISaleDetailRepo
{
  Task<PagedList<SaleDetailModel>> GetByIDAsync(int saleID, SaleDetailParam saleDetailParam, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<SaleDetailModel>> GetListByConditionAsync(Expression<Func<SaleDetailModel, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
  void DeleteEntityRange(IEnumerable<SaleDetailModel> entities);
}
