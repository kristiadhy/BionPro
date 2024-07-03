using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Net.Http.Headers;
using System.Text.Json;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/products/upload")]
public class ProductUploadController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly string folderOnServerName = Path.Combine("StaticFiles", "Images", "Products");

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
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderOnServerName, fileName);

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


    [HttpPost(Name = "UploadProduct")]
    public IActionResult Upload()
    {
        var file = Request.Form.Files[0];

        var fullPathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderOnServerName);

        if (file.Length > 0)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)!.FileName!.Trim('"');

            var fullImagePath = Path.Combine(fullPathToSave, fileName);
            var fullServerImagePathName = Path.Combine(folderOnServerName, fileName);

            using (var stream = new FileStream(fullImagePath, FileMode.Create))
                file.CopyTo(stream);

            return Ok(fullServerImagePathName);
        }
        else
        {
            return BadRequest();
        }
    }
}
