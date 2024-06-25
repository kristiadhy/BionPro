using Domain.Entities;

namespace Persistence.Extensions;
public static class ProductCategoryQueryExtension
{
    public static IQueryable<ProductCategoryModel> SearchByName(this IQueryable<ProductCategoryModel> productCategories, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return productCategories;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return productCategories.Where(e => e.Name!.ToLower().Contains(lowerCaseTerm));
    }
}
