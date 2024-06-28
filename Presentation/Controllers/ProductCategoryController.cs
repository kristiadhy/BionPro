using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;
using Presentation.ActionFilters;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers;

[ApiVersion("1.0")]
[ApiController]
[Route("api/product-categories")]
public class ProductCategoryController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet(Name = "ProductCategories")]
    public async Task<IActionResult> GetByParameters(int productCategoryID, [FromQuery] ProductCategoryParam productCategoryParam, CancellationToken cancellationToken)
    {
        var pagedResult = await _serviceManager.ProductCategoryService.GetByParametersAsync(productCategoryID, productCategoryParam, false, cancellationToken);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
        return Ok(pagedResult.productCategoryDto);
    }


    [HttpGet("{id:int}", Name = "ProductCategoryByID")]
    public async Task<IActionResult> GetByID(int id, CancellationToken cancellationToken)
    {
        var supplier = await _serviceManager.ProductCategoryService.GetByProductCategoryIDAsync(id, false, cancellationToken);
        return Ok(supplier);
    }

    [HttpPost(Name = "CreateProductCategory")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Create([FromBody] ProductCategoryDto productCategoryDto, CancellationToken cancellationToken)
    {
        var createdProductCategory = await _serviceManager.ProductCategoryService.CreateAsync(productCategoryDto, false, cancellationToken);
        return CreatedAtRoute("ProductCategoryByID", new { id = createdProductCategory.CategoryID }, createdProductCategory);
    }

    [HttpPut(Name = "UpdateProductCategory")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Update([FromBody] ProductCategoryDto productCategoryDto, CancellationToken cancellationToken)
    {
        await _serviceManager.ProductCategoryService.UpdateAsync(productCategoryDto, true, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "PartiallyUpdateProductCategory")]
    public async Task<IActionResult> PartiallyUpdateProductCategory(int id, [FromBody] JsonPatchDocument<ProductCategoryDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result = await _serviceManager.ProductCategoryService.GetProductCategoryForPatchAsync(id, true);
        patchDoc.ApplyTo(result.productCategoryToPatch);
        await _serviceManager.ProductCategoryService.SaveChangesForPatchAsync(result.productCategoryToPatch, result.productCategory);
        return NoContent();
    }

    [HttpDelete("{id:guid}", Name = "DeleteProductCategory")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _serviceManager.ProductCategoryService.DeleteAsync(id, false, cancellationToken);
        return NoContent();
    }
}
