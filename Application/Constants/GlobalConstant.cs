using Domain.Helpers;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Constants;
public class GlobalConstant
{
    public static IEnumerable<TransactionCodeHelper> TransactionCodes =
    [
        new() { ID =1, Name ="Purchase", Prefix="PCS" },
    ];
}
