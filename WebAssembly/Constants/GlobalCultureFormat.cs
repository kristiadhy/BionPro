using System.Globalization;

namespace WebAssembly.Constants;

public class GlobalCultureFormat
{
  public static void SetGlobalDateTimeAndDecimalFormat()
  {
    CultureInfo newCulture = new CultureInfo("id-ID");
    newCulture.DateTimeFormat.ShortDatePattern = "dd-MM-yyyy";
    newCulture.DateTimeFormat.ShortTimePattern = "HH:mm:ss";
    newCulture.DateTimeFormat.DateSeparator = "-";
    newCulture.NumberFormat.NumberDecimalSeparator = ",";
    newCulture.NumberFormat.NumberGroupSeparator = ".";
    newCulture.NumberFormat.CurrencySymbol = "Rp. ";

    CultureInfo.DefaultThreadCurrentCulture = newCulture;
    CultureInfo.DefaultThreadCurrentUICulture = newCulture;
  }
}
