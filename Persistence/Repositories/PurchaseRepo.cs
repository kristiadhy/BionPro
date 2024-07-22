using Application.IRepositories;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;
using System.Linq.Expressions;

namespace Persistence.Repositories;
public class PurchaseRepo : MethodBase<PurchaseModel>, IPurchaseRepo
{
    protected new AppDBContext appDBContext;

    public PurchaseRepo(AppDBContext repositoryContext) : base(repositoryContext)
    {
        appDBContext = repositoryContext;
    }

    public async Task<PagedList<PurchaseDtoForSummary>> GetSummaryByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await GetPurchaseWithSummary()
            .SearchBySupplierIDForSummary(purchaseParam.SrcSupplierID)
            .SearchBySupplierForSummary(purchaseParam.SrcSupplierName)
            .SearchByTransactionDateForSummary(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
            .SortForSummary(purchaseParam.OrderBy)
            .Skip((purchaseParam.PageNumber - 1) * purchaseParam.PageSize)
            .Take(purchaseParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await GetPurchaseWithSummary()
            .SearchBySupplierIDForSummary(purchaseParam.SrcSupplierID)
            .SearchBySupplierForSummary(purchaseParam.SrcSupplierName)
            .SearchByTransactionDateForSummary(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
            .CountAsync(cancellationToken);

        return new PagedList<PurchaseDtoForSummary>(purchases, count, purchaseParam.PageNumber, purchaseParam.PageSize);
    }

    //public async Task<PagedList<PurchaseModel>> GetByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default)
    //{
    //    var purchases = await FindAll(trackChanges)
    //        .Include(x => x.Supplier)
    //        .Include(x => x.PurchaseDetails)
    //        //.SearchBySupplier(purchaseParam.SrcSupplier)
    //        //.SearchByTransactionDate(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
    //        .Sort(purchaseParam.OrderBy)
    //        .Skip((purchaseParam.PageNumber - 1) * purchaseParam.PageSize)
    //        .Take(purchaseParam.PageSize)
    //        .ToListAsync(cancellationToken);

    //    var count = await FindAll(trackChanges)
    //        //.Include(x => x.Supplier)
    //        //.Include(x => x.PurchaseDetails)
    //        //.SearchBySupplier(purchaseParam.SrcSupplier)
    //        //.SearchByTransactionDate(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
    //        .CountAsync(cancellationToken);

    //    return new PagedList<PurchaseModel>(purchases, count, purchaseParam.PageNumber, purchaseParam.PageSize);
    //}

    public async Task<PurchaseModel?> GetByIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await FindByCondition(x => x.PurchaseID == purchaseID, trackChanges)
            .Include(x => x.Supplier)
            .Include(x => x.PurchaseDetails!)
            .ThenInclude(pd => pd.Product)
            .FirstOrDefaultAsync(cancellationToken);
        if (purchase is not null)
            return purchase;
        else
            return null;
    }

    public async Task<PurchaseModel?> GetByConditionAsync(Expression<Func<PurchaseModel, bool>> expression, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await FindByCondition(expression, trackChanges)
            .Include(x => x.Supplier)
            .Include(x => x.PurchaseDetails!)
            .ThenInclude(pd => pd.Product)
            .FirstOrDefaultAsync(cancellationToken);
        if (purchase is not null)
            return purchase;
        else
            return null;
    }

    public async Task<bool> CheckTransactionCodeExistAsync(string transactionCode, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await FindByCondition(x => x.TransactionCode == transactionCode, trackChanges)
            .FirstOrDefaultAsync(cancellationToken);
        if (purchase is not null)
            return true;
        else
            return false;
    }

    public void CreateEntity(PurchaseModel entity)
    {
        Create(entity);
    }

    public void UpdateEntity(PurchaseModel entity)
    {
        Update(entity);
    }

    public void DeleteEntity(PurchaseModel entity)
    {
        Delete(entity);
    }

    public void AttachEntity(PurchaseModel entity)
    {
        Attach(entity);
    }

    private IQueryable<PurchaseDtoForSummary> GetPurchaseWithSummary()
    {
        var result = appDBContext.Set<PurchaseModel>().AsNoTracking()
            .Select(a => new PurchaseDtoForSummary
            {
                PurchaseID = a.PurchaseID,
                TransactionCode = a.TransactionCode,
                Date = a.Date,
                DiscountPercentage = a.DiscountPercentage,
                DiscountAmount = a.DiscountAmount,
                Description = a.Description,
                SupplierID = a.Supplier!.SupplierID,
                SupplierName = a.Supplier!.SupplierName,
                TotalItems = a.PurchaseDetails!.Count(),
                TotalQuantity = a.PurchaseDetails!.Sum(pd => pd.Quantity),
                GrandTotal = a.PurchaseDetails!.Sum(pd => pd.SubTotal)
            });

        return result;
    }
}
