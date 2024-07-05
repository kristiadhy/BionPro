using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class ProductQueryExtension
{
    public static IQueryable<ProductModel> SearchByName(this IQueryable<ProductModel> products, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return products;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return products.Where(e => e.Name.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<ProductModel> Sort(this IQueryable<ProductModel> products, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return products.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<SupplierModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return products.OrderBy(e => e.Name);

        return products.OrderBy(orderQuery);
    }
}
