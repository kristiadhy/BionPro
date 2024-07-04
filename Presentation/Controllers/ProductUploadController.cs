using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Net.Http.Headers;

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
    public IActionResult GetProductImage(string fileName)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderPathOnServer, fileName);

        if (System.IO.File.Exists(fullPath))
        {
            var fileBytes = System.IO.File.ReadAllBytes(fullPath);
            return File(fileBytes, "image/png");
        }
        else
        {
            return NotFound();
        }
    }


    [HttpPost(Name = "UploadProductImage")]
    public IActionResult UploadProductImage()
    {
        var file = Request.Form.Files[0];

        var fullPathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPathOnServer);

        if (file.Length > 0)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)!.FileName!.Trim('"');

            var fullImagePathToSave = Path.Combine(fullPathToSave, fileName);

            using (var stream = new FileStream(fullImagePathToSave, FileMode.Create))
                file.CopyTo(stream);

            // Construct the URL to access the image
            var request = HttpContext.Request;
            var imageUrl = $"{folderUrlToServer}/{fileName}";

            return Ok(imageUrl);
        }
        else
        {
            return BadRequest();
        }
    }

    [HttpDelete("{fileName}", Name = "DeleteProductImage")]
    public IActionResult DeleteProductImage(string fileName)
    {
        // Construct the full path to the image file on the server
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderPathOnServer, fileName);

        // Check if the file exists
        if (System.IO.File.Exists(fullPath))
        {
            // Delete the file
            System.IO.File.Delete(fullPath);

            // Return a success response
            return Ok();
        }
        else
        {
            // If the file does not exist, return a Not Found response
            return NotFound();
        }
    }
}
