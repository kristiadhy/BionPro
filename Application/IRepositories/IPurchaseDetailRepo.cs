using Domain.Entities;
using Domain.Parameters;
using System.Linq.Expressions;

namespace Application.IRepositories;
public interface IPurchaseDetailRepo : IRepositoryBase<PurchaseDetailModel>
{
  Task<PagedList<PurchaseDetailModel>> GetByIDAsync(int purchaseID, PurchaseDetailParam purchaseDetailParam, bool trackChanges, CancellationToken cancellationToken = default);
  Task<IEnumerable<PurchaseDetailModel>> GetListByConditionAsync(Expression<Func<PurchaseDetailModel, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default);
  void DeleteEntityRange(IEnumerable<PurchaseDetailModel> entities);
}
