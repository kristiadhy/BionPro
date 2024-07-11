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

    public async Task<PagedList<PurchaseModel>> GetByParametersAsync(PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await FindAll(trackChanges)
            .Include(x => x.Supplier)
            .Include(x => x.PurchaseDetails)
            .SearchBySupplier(purchaseParam.SrcSupplier)
            .SearchByTransactionDate(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
            .Sort(purchaseParam.OrderBy)
            .Skip((purchaseParam.PageNumber - 1) * purchaseParam.PageSize)
            .Take(purchaseParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges)
            .Include(x => x.Supplier)
            .Include(x => x.PurchaseDetails)
            .SearchBySupplier(purchaseParam.SrcSupplier)
            .SearchByTransactionDate(purchaseParam.SrcDateFrom, purchaseParam.SrcDateTo)
            .CountAsync(cancellationToken);

        return new PagedList<PurchaseModel>(purchases, count, purchaseParam.PageNumber, purchaseParam.PageSize);
    }

    public async Task<PurchaseModel?> GetByIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await FindByCondition(x => x.PurchaseID == purchaseID, trackChanges).FirstOrDefaultAsync(cancellationToken);
        if (purchase is not null)
            return purchase;
        else
            return null;
    }

    public async Task<bool> CheckTransactionCodeExistAsync(string transactionCode, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await FindByCondition(x => x.TransactionCode == transactionCode, trackChanges).FirstOrDefaultAsync(cancellationToken);
        if (purchase is not null)
            return true;
        else
            return false;
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
        var result = appDBContext.Set<PurchaseModel>().AsNoTracking()
            .Select(a => new PurchaseDtoForQueries
            {
                PurchaseID = a.PurchaseID,
                TransactionCode = a.TransactionCode,
                Date = a.Date,
                DiscountPercentage = a.DiscountPercentage,
                DiscountAmount = a.DiscountAmount,
                Description = a.Description,
                SupplierName = a.Supplier!.SupplierName, // Assuming navigation property is properly configured
                GrandTotal = a.PurchaseDetails!.Sum(pd => pd.SubTotal) // Directly summing the SubTotal of related PurchaseDetails
            });

        return result;
    }
}
