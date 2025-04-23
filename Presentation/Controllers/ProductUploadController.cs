using Microsoft.AspNetCore.Mvc;
using Services.Contracts;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/products/upload")]
public class ProductUploadController(IServiceManager serviceManager) : ControllerBase
{
  private readonly IServiceManager _serviceManager = serviceManager;
  private readonly string folderPathOnServer = Path.Combine("StaticFiles", "Images", "Products");
  private readonly string folderUrlToServer = "StaticFiles/Images/Products";

  //[HttpGet(Name = "GetProductImage")]
  //public async Task<IActionResult> GetProductImage(Guid productID, CancellationToken cancellationToken)
  //{
  //    var product = await _serviceManager.ProductService.GetByProductIDAsync(productID, false, cancellationToken);
  //    if (product.ImageUrl is not null)
  //    {
  //        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderOnServerName, product.ImageUrl);
  //        if (System.IO.File.Exists(fullPath))
  //        {
  //            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
  //            return File(fileBytes, "image/png");
  //        }
  //    }

  //    return NotFound();
  //}

  [HttpGet("{fileName}", Name = "GetProductImage")]
  public async Task<IActionResult> GetProductImage(string fileName)
  {
    var fileBytes = await _serviceManager.ProductService.GetProductImage(fileName);
    if (fileBytes is not null)
      return File(fileBytes, "image/png");
    else
      return NotFound();
  }


  [HttpPost(Name = "UploadProductImage")]
  public async Task<IActionResult> UploadProductImage()
  {
    var file = Request.Form.Files[0];
    var imageUrl = await _serviceManager.ProductService.UploadProductImage(file);
    return Ok(imageUrl);
  }

  [HttpDelete("{fileName}", Name = "DeleteProductImage")]
  public async Task<IActionResult> DeleteProductImage(string fileName)
  {
    await _serviceManager.ProductService.DeleteProductImage(fileName);
    return NoContent();
  }
}
