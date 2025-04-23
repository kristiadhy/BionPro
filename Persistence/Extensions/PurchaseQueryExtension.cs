using Domain.DTO;
using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class PurchaseQueryExtension
{
  public static IQueryable<PurchaseDtoForSummary> SearchBySupplierIDForSummary(this IQueryable<PurchaseDtoForSummary> purchases, Guid? supplierID)
  {
    if (supplierID is null)
      return purchases;

    return purchases.Where(e => e.SupplierID == supplierID);
  }

  public static IQueryable<PurchaseDtoForSummary> SearchBySupplierForSummary(this IQueryable<PurchaseDtoForSummary> purchases, string? searchTerm)
  {
    if (string.IsNullOrWhiteSpace(searchTerm))
      return purchases;

    var lowerCaseTerm = searchTerm.Trim().ToLower();

    return purchases.Where(e => e.SupplierName!.Contains(lowerCaseTerm));
  }

  public static IQueryable<PurchaseDtoForSummary> SearchByTransactionDateForSummary(this IQueryable<PurchaseDtoForSummary> purchases, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
  {
    //IMPORTANT : When dealing with DateTimeOffset, the .Date property converts the instance to the local time zone before extracting the date part. This means that if dateFrom and dateTo are in different time zones or if the Date property of the SaleDtoForSummary objects is in a different time zone, the comparison is still valid because it's based on the date component only, after converting to the local time zone.

    if (dateFrom is null || dateTo is null)
      return purchases;

    // Convert dateFrom and dateTo to their Date components to exclude time
    var dateFromWithoutTime = dateFrom.Value.Date;
    var dateToWithoutTime = dateTo.Value.Date;

    // Adjust the query to compare only the date parts
    return purchases.Where(e => e.Date.Date >= dateFromWithoutTime && e.Date.Date <= dateToWithoutTime);
  }

  public static IQueryable<PurchaseDtoForSummary> SortForSummary(this IQueryable<PurchaseDtoForSummary> purchases, string? orderByQueryString)
  {
    if (string.IsNullOrWhiteSpace(orderByQueryString))
      return purchases.OrderBy(e => e.Date);

    var orderQuery = OrderQueryBuilder.CreateOrderQuery<PurchaseDtoForSummary>(orderByQueryString);

    if (string.IsNullOrWhiteSpace(orderQuery))
      return purchases.OrderBy(e => e.Date);

    return purchases.OrderBy(orderQuery);
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
