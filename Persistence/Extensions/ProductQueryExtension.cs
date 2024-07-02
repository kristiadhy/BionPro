using Domain.Entities;

namespace Persistence.Extensions;
public static class ProductQueryExtension
{
    public static IQueryable<ProductModel> SearchByName(this IQueryable<ProductModel> productCategories, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return productCategories;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return productCategories.Where(e => e.Name!.ToLower().Contains(lowerCaseTerm));
    }
}
