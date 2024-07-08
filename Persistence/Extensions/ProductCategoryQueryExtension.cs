using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class ProductCategoryQueryExtension
{
    public static IQueryable<ProductCategoryModel> SearchByName(this IQueryable<ProductCategoryModel> productCategories, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return productCategories;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return productCategories.Where(e => e.Name!.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<ProductCategoryModel> Sort(this IQueryable<ProductCategoryModel> productCategories, string? orderByQueryString)
    {
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return productCategories.OrderBy(e => e.Name);

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<ProductCategoryModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return productCategories.OrderBy(e => e.Name);

        return productCategories.OrderBy(orderQuery);
    }
}
