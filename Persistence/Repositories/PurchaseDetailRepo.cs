using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;

namespace Persistence.Repositories;
public class PurchaseDetailRepo : MethodBase<PurchaseDetailModel>, IPurchaseDetailRepo
{
    public PurchaseDetailRepo(AppDBContext repositoryContext) : base(repositoryContext)
    {
        appDBContext = repositoryContext;
    }

    public async Task<PagedList<PurchaseDetailModel>> GetByIDAsync(int purchaseID, PurchaseDetailParam purchaseDetailParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await FindByCondition(x => x.PurchaseID == purchaseID, trackChanges)
            .Include(x => x.Product)
            .Sort(purchaseDetailParam.OrderBy)
            .Skip((purchaseDetailParam.PageNumber - 1) * purchaseDetailParam.PageSize)
            .Take(purchaseDetailParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await FindByCondition(x => x.PurchaseID == purchaseID, trackChanges)
            .CountAsync(cancellationToken);

        return new PagedList<PurchaseDetailModel>(purchases, count, purchaseDetailParam.PageNumber, purchaseDetailParam.PageSize);
    }

    public async Task<IEnumerable<PurchaseDetailModel>> GetListByConditionAsync(Expression<Func<PurchaseDetailModel, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await FindByCondition(expression, trackChanges)
            .ToListAsync(cancellationToken);

        return purchases;
    }

    public void CreateEntity(PurchaseDetailModel entity)
    {
        Create(entity);
    }

    public void UpdateEntity(PurchaseDetailModel entity)
    {
        Update(entity);
    }

    public void DeleteEntity(PurchaseDetailModel entity)
    {
        Delete(entity);
    }

    public void DeleteEntityRange(IEnumerable<PurchaseDetailModel> entities)
    {
        DeleteRange(entities);
    }
}
