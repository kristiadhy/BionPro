using Application.IRepositories;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;
public class PurchaseRepo : MethodBase<PurchaseModel>, IPurchaseRepo
{
    protected new AppDBContext appDBContext;

    public PurchaseRepo(AppDBContext repositoryContext) : base(repositoryContext)
    {
        appDBContext = repositoryContext;
    }

    public async Task<PagedList<PurchaseDtoForQueries>> GetByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await GetPurchaseWithSummary()
            .SearchBySupplier(purchaseParam.SrcSupplier)
            .SearchByTransactionDate(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
            .Sort(purchaseParam.OrderBy)
            .Skip((purchaseParam.PageNumber - 1) * purchaseParam.PageSize)
            .Take(purchaseParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await GetPurchaseWithSummary()
            .SearchBySupplier(purchaseParam.SrcSupplier)
            .SearchByTransactionDate(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
            .CountAsync(cancellationToken);

        return new PagedList<PurchaseDtoForQueries>(purchases, count, purchaseParam.PageNumber, purchaseParam.PageSize);
    }

    public async Task<PurchaseModel?> GetByIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await FindByCondition(x => x.PurchaseID == purchaseID, trackChanges).FirstOrDefaultAsync(cancellationToken);
        if (purchase is not null)
            return purchase;
        else
            return null;
    }

    public void CreateEntity(PurchaseModel entity, bool trackChanges)
    {
        Create(entity);
    }

    public void UpdateEntity(PurchaseModel entity, bool trackChanges)
    {
        Update(entity);
    }

    public void DeleteEntity(PurchaseModel entity, bool trackChanges)
    {
        Delete(entity);
    }

    private IQueryable<PurchaseDtoForQueries> GetPurchaseWithSummary()
    {
        var result = from a in appDBContext.Set<PurchaseModel>().AsNoTracking()
                     join b in (
            from b in appDBContext.Set<PurchaseDetailModel>().AsNoTracking()
            group b by b.PurchaseID into g
            select new
            {
                PurchaseID = g.Key,
                GrandTotal = g.Sum(x => x.SubTotal)
            }
        ) on a.PurchaseID equals b.PurchaseID
                     select new PurchaseDtoForQueries
                     {
                         PurchaseID = a.PurchaseID,
                         TransactionCode = a.TransactionCode,
                         Date = a.Date,
                         DiscountPercentage = a.DiscountPercentage,
                         DiscountAmount = a.DiscountAmount,
                         Description = a.Description,
                         SupplierName = a.Supplier!.SupplierName,
                         GrandTotal = b.GrandTotal
                     };

        return result;
    }
}
