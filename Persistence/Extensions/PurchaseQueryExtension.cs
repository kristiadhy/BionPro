using Domain.DTO;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class PurchaseQueryExtension
{
    public static IQueryable<PurchaseDtoForQueries> SearchBySupplier(this IQueryable<PurchaseDtoForQueries> purchases, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return purchases;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return purchases.Where(e => e.SupplierName!.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<PurchaseDtoForQueries> SearchByTransactionDate(this IQueryable<PurchaseDtoForQueries> purchases, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
    {
        if (dateFrom is null || dateTo is null)
            return purchases;

        return purchases.Where(e => e.Date >= dateFrom && e.Date <= dateTo);
    }

    public static IQueryable<PurchaseDtoForQueries> Sort(this IQueryable<PurchaseDtoForQueries> purchases, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return purchases.OrderBy(e => e.Date);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<PurchaseDtoForQueries>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return purchases.OrderBy(e => e.Date);

        return purchases.OrderBy(orderQuery);
    }
}
