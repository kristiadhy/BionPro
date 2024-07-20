using Domain.Helpers;

namespace Application.Constants;
public class GlobalConstant
{
    public static IEnumerable<TransactionCodeHelper> TransactionCodes =
    [
        new() { ID =1, Name ="Purchase", Prefix="PCS" },
        new() { ID =2, Name ="Sale", Prefix="SLS" },
    ];
}
