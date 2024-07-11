using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Factories;
public class TransactionCodeFactory
{
    public static string GenerateTransactionCode(string prefix)
    {
        if (string.IsNullOrWhiteSpace(prefix) || prefix.Length != 3)
        {
            throw new ArgumentException("Prefix must be exactly 3 characters long.", nameof(prefix));
        }

        // Get the current date in the format of "yyMMdd"
        string datePart = DateTime.Now.ToString("ddMMyy", CultureInfo.InvariantCulture);

        // Generate a random 6-digit number
        Random random = new Random();
        string randomPart = random.Next(100000, 999999).ToString();

        // Combine the parts to form the transaction code
        string transactionCode = $"{prefix.ToUpper()}{datePart}{randomPart}";

        if (transactionCode.Length != 15)
        {
            throw new InvalidOperationException("The generated transaction code does not meet the required length.");
        }

        return transactionCode;
    }
}
