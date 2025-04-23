using SixLabors.ImageSharp;
using ZXing;
using ZXing.Common;

namespace BarcodeService;
public class BarcodeService
{
  public byte[] GenerateEAN13Barcode(string eanCode)
  {
    if (eanCode.Length != 13 || !IsNumeric(eanCode))
    {
      throw new ArgumentException("EAN-13 code must be a 13-digit numeric string.");
    }

    var barcodeWriter = new BarcodeWriterPixelData
    {
      Format = BarcodeFormat.EAN_13,
      Options = new EncodingOptions
      {
        Height = 150,
        Width = 300,
        Margin = 10
      }
    };

    var pixelData = barcodeWriter.Write(eanCode);

    using var bitmap = new Image<SixLabors.ImageSharp.PixelFormats.Rgba32>(pixelData.Width, pixelData.Height);
    for (int y = 0; y < pixelData.Height; y++)
    {
      for (int x = 0; x < pixelData.Width; x++)
      {
        var color = pixelData.Pixels[(y * pixelData.Width + x) * 4];
        bitmap[x, y] = new SixLabors.ImageSharp.PixelFormats.Rgba32(color, color, color);
      }
    }

    using (var stream = new MemoryStream())
    {
      bitmap.SaveAsPng(stream);
      return stream.ToArray();
    }
  }

  private bool IsNumeric(string value)
  {
    return long.TryParse(value, out _);
  }
}
