using Domain.DTO;
using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class SaleQueryExtension
{
    public static IQueryable<SaleDtoForSummary> SearchByCustomerIDForSummary(this IQueryable<SaleDtoForSummary> purchases, Guid? customerID)
    {
        if (customerID is null)
            return purchases;

        return purchases.Where(e => e.CustomerID == customerID);
    }

    public static IQueryable<SaleDtoForSummary> SearchByCustomerForSummary(this IQueryable<SaleDtoForSummary> purchases, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return purchases;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return purchases.Where(e => e.CustomerName!.Contains(lowerCaseTerm));
    }

    public static IQueryable<SaleDtoForSummary> SearchByTransactionDateForSummary(this IQueryable<SaleDtoForSummary> purchases, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
    {
        if (dateFrom is null || dateTo is null)
            return purchases;

        return purchases.Where(e => e.Date >= dateFrom && e.Date <= dateTo);
    }

    public static IQueryable<SaleDtoForSummary> SortForSummary(this IQueryable<SaleDtoForSummary> purchases, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return purchases.OrderBy(e => e.Date);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<SaleDtoForSummary>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return purchases.OrderBy(e => e.Date);

        return purchases.OrderBy(orderQuery);
    }

    public static IQueryable<SaleModel> Sort(this IQueryable<SaleModel> purchases, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return purchases.OrderBy(e => e.Date);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<SaleModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return purchases.OrderBy(e => e.Date);

        return purchases.OrderBy(orderQuery);
    }
}
