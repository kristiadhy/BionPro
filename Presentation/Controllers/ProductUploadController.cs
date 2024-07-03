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

    [HttpPost(Name = "UploadProduct")]
    public IActionResult Upload(IFormFile file)
    {
        //var file = Request.Form.Files[0];

        var folderName = Path.Combine("StaticFiles", "Images", "Products");

        var pathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderName);

        if (file.Length > 0)
        {
            var fileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)!.FileName!.Trim('"');

            var fullPath = Path.Combine(pathToSave, fileName);
            var dbPath = Path.Combine(folderName, fileName);

            using (var stream = new FileStream(fullPath, FileMode.Create))
                file.CopyTo(stream);

            return Ok(dbPath);
        }
        else
        {
            return BadRequest();
        }
    }
}
