using Domain.Entities;
using System.Linq.Dynamic.Core;

namespace Persistence.Extensions;
public static class CustomerQueryExtensions
{
    //public static IQueryable<CustomerMD> FilterByAge(this IQueryable<CustomerMD> customers, uint minAge, uint maxAge) 
    //    => customers.Where(e => (e.Age >= minAge && e.Age <= maxAge));

    public static IQueryable<CustomerModel> SearchByName(this IQueryable<CustomerModel> customers, string? searchTerm)
    {
        if (string.IsNullOrWhiteSpace(searchTerm))
            return customers;

        var lowerCaseTerm = searchTerm.Trim().ToLower();

        return customers.Where(e => e.CustomerName!.Contains(lowerCaseTerm, StringComparison.CurrentCultureIgnoreCase));
    }

    public static IQueryable<CustomerModel> Sort(this IQueryable<CustomerModel> customers, string? orderByQueryString)
    {
        //The example format of orderByQueryString is "name, age desc"
        if (string.IsNullOrWhiteSpace(orderByQueryString))
            return customers.OrderBy(e => e.CustomerName); //If there is no order by, then sort by customer name

        var orderQuery = OrderQueryBuilder.CreateOrderQuery<CustomerModel>(orderByQueryString);

        if (string.IsNullOrWhiteSpace(orderQuery))
            return customers.OrderBy(e => e.CustomerName);

        return customers.OrderBy(orderQuery);
    }
}
