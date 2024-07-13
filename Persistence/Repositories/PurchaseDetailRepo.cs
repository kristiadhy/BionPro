using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

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
}
