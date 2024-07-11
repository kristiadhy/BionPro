using Domain.DTO;
using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class PurchaseQueryExtension
{
    public static IQueryable<PurchaseModel> SearchBySupplier(this IQueryable<PurchaseModel> purchases, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return purchases;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return purchases.Where(e => e.Supplier!.SupplierName!.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<PurchaseModel> SearchByTransactionDate(this IQueryable<PurchaseModel> purchases, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
    {
        if (dateFrom is null || dateTo is null)
            return purchases;

        return purchases.Where(e => e.Date >= dateFrom && e.Date <= dateTo);
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
