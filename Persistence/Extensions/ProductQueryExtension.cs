using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class ProductQueryExtension
{
    public static IQueryable<ProductModel> SearchByProductCategory(this IQueryable<ProductModel> products, int? productCategoryID)
    {
        if (productCategoryID is null)
            return products;

        return products.Where(e => e.CategoryID == productCategoryID);
    }

    public static IQueryable<ProductModel> SearchByProductName(this IQueryable<ProductModel> products, string? productname)
    {
        if (string.IsNullOrWhiteSpace(productname))
            return products;

        var lowerCaseTerm = productname.Trim().ToLower();

        return products.Where(e => e.Name!.Contains(lowerCaseTerm));
    }

    public static IQueryable<ProductModel> Sort(this IQueryable<ProductModel> products, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return products.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<ProductModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return products.OrderBy(e => e.Name);

        return products.OrderBy(orderQuery);
    }
}
