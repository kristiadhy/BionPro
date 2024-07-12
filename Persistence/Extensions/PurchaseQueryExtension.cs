using Domain.DTO;
using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class PurchaseQueryExtension
{
    public static IQueryable<PurchaseDtoForSummary> SearchBySupplierForSummary(this IQueryable<PurchaseDtoForSummary> purchases, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return purchases;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return purchases.Where(e => e.SupplierName!.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<PurchaseDtoForSummary> SearchByTransactionDateForSummary(this IQueryable<PurchaseDtoForSummary> purchases, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
    {
        if (dateFrom is null || dateTo is null)
            return purchases;

        return purchases.Where(e => e.Date >= dateFrom && e.Date <= dateTo);
    }

    public static IQueryable<PurchaseDtoForSummary> SortForSummary(this IQueryable<PurchaseDtoForSummary> purchases, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return purchases.OrderBy(e => e.Date);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<PurchaseDtoForSummary>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return purchases.OrderBy(e => e.Date);

        return purchases.OrderBy(orderQuery);
    }

    public static IQueryable<PurchaseModel> Sort(this IQueryable<PurchaseModel> purchases, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return purchases.OrderBy(e => e.Date);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<PurchaseModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return purchases.OrderBy(e => e.Date);

        return purchases.OrderBy(orderQuery);
    }
}
