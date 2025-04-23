using Domain.DTO;
using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class SaleQueryExtension
{
  public static IQueryable<SaleDtoForSummary> SearchByCustomerIDForSummary(this IQueryable<SaleDtoForSummary> sales, Guid? customerID)
  {
    if (customerID is null)
      return sales;

    return sales.Where(e => e.CustomerID == customerID);
  }

  public static IQueryable<SaleDtoForSummary> SearchByCustomerForSummary(this IQueryable<SaleDtoForSummary> sales, string? searchTerm)
  {
    if (string.IsNullOrWhiteSpace(searchTerm))
      return sales;

    var lowerCaseTerm = searchTerm.Trim().ToLower();

    return sales.Where(e => e.CustomerName!.Contains(lowerCaseTerm));
  }

  public static IQueryable<SaleDtoForSummary> SearchByTransactionDateForSummary(this IQueryable<SaleDtoForSummary> sales, DateTimeOffset? dateFrom, DateTimeOffset? dateTo)
  {
    //IMPORTANT : When dealing with DateTimeOffset, the .Date property converts the instance to the local time zone before extracting the date part. This means that if dateFrom and dateTo are in different time zones or if the Date property of the SaleDtoForSummary objects is in a different time zone, the comparison is still valid because it's based on the date component only, after converting to the local time zone.

    if (dateFrom is null || dateTo is null)
      return sales;

    // Convert dateFrom and dateTo to their Date components to exclude time
    var dateFromWithoutTime = dateFrom.Value.Date;
    var dateToWithoutTime = dateTo.Value.Date;

    // Adjust the query to compare only the date parts
    return sales.Where(e => e.Date.Date >= dateFromWithoutTime && e.Date.Date <= dateToWithoutTime);
  }

  public static IQueryable<SaleDtoForSummary> SortForSummary(this IQueryable<SaleDtoForSummary> sales, string? orderByQueryString)
  {
    if (string.IsNullOrWhiteSpace(orderByQueryString))
      return sales.OrderBy(e => e.Date);

    var orderQuery = OrderQueryBuilder.CreateOrderQuery<SaleDtoForSummary>(orderByQueryString);

    if (string.IsNullOrWhiteSpace(orderQuery))
      return sales.OrderBy(e => e.Date);

    return sales.OrderBy(orderQuery);
  }

  public static IQueryable<SaleModel> Sort(this IQueryable<SaleModel> sales, string? orderByQueryString)
  {
    if (string.IsNullOrWhiteSpace(orderByQueryString))
      return sales.OrderBy(e => e.Date);

    var orderQuery = OrderQueryBuilder.CreateOrderQuery<SaleModel>(orderByQueryString);

    if (string.IsNullOrWhiteSpace(orderQuery))
      return sales.OrderBy(e => e.Date);

    return sales.OrderBy(orderQuery);
  }
}
