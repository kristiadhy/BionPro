using Application.IRepositories;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;
public class SaleRepo : MethodBase<SaleModel>, ISaleRepo
{
    protected new AppDBContext appDBContext;

    public SaleRepo(AppDBContext repositoryContext) : base(repositoryContext)
    {
        appDBContext = repositoryContext;
    }

    public async Task<PagedList<SaleDtoForSummary>> GetSummaryByParametersAsync(SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var sales = await GetSaleWithSummary()
            .SearchByCustomerIDForSummary(saleParam.SrcCustomerID)
            .SearchByCustomerForSummary(saleParam.SrcCustomerName)
            .SearchByTransactionDateForSummary(saleParam.SrcDateFrom, saleParam.SrcDateTo)
            .SortForSummary(saleParam.OrderBy)
            .Skip((saleParam.PageNumber - 1) * saleParam.PageSize)
            .Take(saleParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await GetSaleWithSummary()
            .SearchByCustomerIDForSummary(saleParam.SrcCustomerID)
            .SearchByCustomerForSummary(saleParam.SrcCustomerName)
            .SearchByTransactionDateForSummary(saleParam.SrcDateFrom, saleParam.SrcDateTo)
            .CountAsync(cancellationToken);

        return new PagedList<SaleDtoForSummary>(sales, count, saleParam.PageNumber, saleParam.PageSize);
    }

    //public async Task<PagedList<SaleModel>> GetByParametersAsync(SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default)
    //{
    //    var sales = await FindAll(trackChanges)
    //        .Include(x => x.Customer)
    //        .Include(x => x.SaleDetails)
    //        //.SearchByCustomer(saleParam.SrcCustomer)
    //        //.SearchByTransactionDate(saleParam.SrcDateFrom, saleParam.SrcDateTo)
    //        .Sort(saleParam.OrderBy)
    //        .Skip((saleParam.PageNumber - 1) * saleParam.PageSize)
    //        .Take(saleParam.PageSize)
    //        .ToListAsync(cancellationToken);

    //    var count = await FindAll(trackChanges)
    //        //.Include(x => x.Customer)
    //        //.Include(x => x.SaleDetails)
    //        //.SearchByCustomer(saleParam.SrcCustomer)
    //        //.SearchByTransactionDate(saleParam.SrcDateFrom, saleParam.SrcDateTo)
    //        .CountAsync(cancellationToken);

    //    return new PagedList<SaleModel>(sales, count, saleParam.PageNumber, saleParam.PageSize);
    //}

    public async Task<SaleModel?> GetByIDAsync(int saleID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var sale = await FindByCondition(x => x.SaleID == saleID, trackChanges)
            .Include(x => x.Customer)
            .Include(x => x.SaleDetails!)
            .ThenInclude(pd => pd.Product)
            .FirstOrDefaultAsync(cancellationToken);
        if (sale is not null)
            return sale;
        else
            return null;
    }

    public async Task<bool> CheckTransactionCodeExistAsync(string transactionCode, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var sale = await FindByCondition(x => x.TransactionCode == transactionCode, trackChanges)
            .FirstOrDefaultAsync(cancellationToken);
        if (sale is not null)
            return true;
        else
            return false;
    }

    public void CreateEntity(SaleModel entity, bool trackChanges)
    {
        Create(entity);
    }

    public void UpdateEntity(SaleModel entity, bool trackChanges)
    {
        Update(entity);
    }

    public void DeleteEntity(SaleModel entity, bool trackChanges)
    {
        Delete(entity);
    }

    private IQueryable<SaleDtoForSummary> GetSaleWithSummary()
    {
        var result = appDBContext.Set<SaleModel>().AsNoTracking()
            .Select(a => new SaleDtoForSummary
            {
                SaleID = a.SaleID,
                TransactionCode = a.TransactionCode,
                Date = a.Date,
                DiscountPercentage = a.DiscountPercentage,
                DiscountAmount = a.DiscountAmount,
                Description = a.Description,
                CustomerID = a.Customer!.CustomerID,
                CustomerName = a.Customer!.CustomerName,
                TotalItems = a.SaleDetails!.Count(),
                TotalQuantity = a.SaleDetails!.Sum(pd => pd.Quantity),
                GrandTotal = a.SaleDetails!.Sum(pd => pd.SubTotal)
            });

        return result;
    }
}
