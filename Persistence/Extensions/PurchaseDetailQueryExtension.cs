using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class PurchaseDetailQueryExtension
{
    public static IQueryable<PurchaseDetailModel> SearchByProduct(this IQueryable<PurchaseDetailModel> products, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return products;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return products.Where(e => e.Product!.Name.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<PurchaseDetailModel> Sort(this IQueryable<PurchaseDetailModel> products, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return products.OrderBy(e => e.Product!.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<PurchaseDetailModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return products.OrderBy(e => e.Product!.Name);

        return products.OrderBy(orderQuery);
    }
}
