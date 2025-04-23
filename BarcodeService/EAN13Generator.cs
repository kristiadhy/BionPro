using System.Text;

namespace BarcodeService;

public class EAN13Generator
{
  public string GenerateRandomEAN13()
  {
    Random random = new Random();
    StringBuilder ean13 = new StringBuilder();

    // Generate the first 12 digits randomly
    for (int i = 0; i < 12; i++)
    {
      ean13.Append(random.Next(0, 10));
    }

    // Calculate the check digit
    int checkDigit = CalculateCheckDigit(ean13.ToString());
    ean13.Append(checkDigit);

    return ean13.ToString();
  }

  private int CalculateCheckDigit(string ean12)
  {
    int sum = 0;

    for (int i = 0; i < ean12.Length; i++)
    {
      int digit = int.Parse(ean12[i].ToString());
      if (i % 2 == 0)
      {
        sum += digit; // odd positions
      }
      else
      {
        sum += digit * 3; // even positions
      }
    }

    int nearestMultipleOfTen = (int)Math.Ceiling(sum / 10.0) * 10;
    int checkDigit = nearestMultipleOfTen - sum;

    return checkDigit;
  }
}

public class Program
{
  public static void Main()
  {
    EAN13Generator generator = new EAN13Generator();
    string randomEAN13 = generator.GenerateRandomEAN13();
    Console.WriteLine("Random EAN-13 Barcode: " + randomEAN13);
  }
}
