using Microsoft.AspNetCore.Components.Forms;

namespace WebAssembly.Services;

public class ImageProcessService
{
  public static async Task<string> DisplayImage(IBrowserFile imageFile, string format = "format/png")
  {
    // Set the ImageUrl to a temporary URL to display the selected image
    var imageStream = imageFile.OpenReadStream(maxAllowedSize: 1024 * 1024 * 15); // Example: Limit file size to 15MB
    var resizedImageFile = await imageFile.RequestImageFileAsync(format, 300, 500); // Resize if needed
    var buffer = new byte[resizedImageFile.Size];
    await resizedImageFile.OpenReadStream().ReadAsync(buffer);
    var base64Image = Convert.ToBase64String(buffer);
    return base64Image;
  }
}
