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
[Route("api/products")]
public class ProductController(IServiceManager serviceManager) : ControllerBase
{
  private readonly IServiceManager _serviceManager = serviceManager;

  [HttpGet(Name = "Product")]
  public async Task<IActionResult> GetByParameters(Guid productID, [FromQuery] ProductParam productParam, CancellationToken cancellationToken)
  {
    var pagedResult = await _serviceManager.ProductService.GetByParametersAsync(productID, productParam, false, cancellationToken);
    Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
    return Ok(pagedResult.productDto);
  }


  [HttpGet("{id:Guid}", Name = "ProductByID")]
  public async Task<IActionResult> GetByID(Guid id, CancellationToken cancellationToken)
  {
    var product = await _serviceManager.ProductService.GetByProductIDAsync(id, false, cancellationToken);
    return Ok(product);
  }

  [HttpPost(Name = "CreateProduct")]
  [ServiceFilter(typeof(ValidationFilterAttribute))]
  public async Task<IActionResult> Create([FromBody] ProductDto productDto, CancellationToken cancellationToken)
  {
    var createdProduct = await _serviceManager.ProductService.CreateAsync(productDto, false, cancellationToken);
    return CreatedAtRoute("ProductByID", new { id = createdProduct.ProductID }, createdProduct);
  }

  [HttpPut(Name = "UpdateProduct")]
  [ServiceFilter(typeof(ValidationFilterAttribute))]
  public async Task<IActionResult> Update([FromBody] ProductDto productDto, CancellationToken cancellationToken)
  {
    await _serviceManager.ProductService.UpdateAsync(productDto, true, cancellationToken);
    return NoContent();
  }

  [HttpPatch("{id:Guid}", Name = "PartiallyUpdateProduct")]
  public async Task<IActionResult> PartiallyUpdateProduct(Guid id, [FromBody] JsonPatchDocument<ProductDto> patchDoc)
  {
    if (patchDoc is null)
      return BadRequest("patchDoc object sent from client is null.");

    var result = await _serviceManager.ProductService.GetProductForPatchAsync(id, true);
    patchDoc.ApplyTo(result.productToPatch);
    await _serviceManager.ProductService.SaveChangesForPatchAsync(result.productToPatch, result.product);
    return NoContent();
  }

  [HttpDelete("{id:guid}", Name = "DeleteProduct")]
  public async Task<IActionResult> Delete(Guid id, CancellationToken cancellationToken)
  {
    await _serviceManager.ProductService.DeleteAsync(id, false, cancellationToken);
    return NoContent();
  }
}
