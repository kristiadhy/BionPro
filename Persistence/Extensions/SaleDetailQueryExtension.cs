using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class SaleDetailQueryExtension
{
  public static IQueryable<SaleDetailModel> SearchByProduct(this IQueryable<SaleDetailModel> products, string? searchTerm)
  {
    if (string.IsNullOrWhiteSpace(searchTerm))
      return products;

    var lowerCaseTerm = searchTerm.Trim().ToLower();

    return products.Where(e => e.Product!.Name.Contains(lowerCaseTerm));
  }

  public static IQueryable<SaleDetailModel> Sort(this IQueryable<SaleDetailModel> products, string? orderByQueryString)
  {
    if (string.IsNullOrWhiteSpace(orderByQueryString))
      return products.OrderBy(e => e.Product!.Name);

    var orderQuery = OrderQueryBuilder.CreateOrderQuery<SaleDetailModel>(orderByQueryString);

    if (string.IsNullOrWhiteSpace(orderQuery))
      return products.OrderBy(e => e.Product!.Name);

    return products.OrderBy(orderQuery);
  }
}
