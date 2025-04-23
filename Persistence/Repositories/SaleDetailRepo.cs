using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;

namespace Persistence.Repositories;
public class SaleDetailRepo : MethodBase<SaleDetailModel>, ISaleDetailRepo
{
  public SaleDetailRepo(AppDBContext repositoryContext) : base(repositoryContext)
  {
    appDBContext = repositoryContext;
  }

  public async Task<PagedList<SaleDetailModel>> GetByIDAsync(int saleID, SaleDetailParam saleDetailParam, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var sales = await FindByCondition(x => x.SaleID == saleID, trackChanges)
        .Include(x => x.Product)
        .Sort(saleDetailParam.OrderBy)
        .Skip((saleDetailParam.PageNumber - 1) * saleDetailParam.PageSize)
        .Take(saleDetailParam.PageSize)
        .ToListAsync(cancellationToken);

    var count = await FindByCondition(x => x.SaleID == saleID, trackChanges)
        .CountAsync(cancellationToken);

    return new PagedList<SaleDetailModel>(sales, count, saleDetailParam.PageNumber, saleDetailParam.PageSize);
  }

  public async Task<IEnumerable<SaleDetailModel>> GetListByConditionAsync(Expression<Func<SaleDetailModel, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var sales = await FindByCondition(expression, trackChanges)
        .ToListAsync(cancellationToken);

    return sales;
  }

  public void DeleteEntityRange(IEnumerable<SaleDetailModel> entities)
  {
    DeleteRange(entities);
  }
}
