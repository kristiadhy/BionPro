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
[Route("api/purchases")]
public class PurchaseController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet(Name = "Purchases")]
    public async Task<IActionResult> GetByParameters(int purchaseID, [FromQuery] PurchaseParam purchaseParam, CancellationToken cancellationToken)
    {
        var pagedResult = await _serviceManager.PurchaseService.GetByParametersAsync(purchaseID, purchaseParam, false, cancellationToken);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
        return Ok(pagedResult.purchaseDto);
    }


    [HttpGet("{id:int}", Name = "PurchaseByID")]
    public async Task<IActionResult> GetByID(int id, CancellationToken cancellationToken)
    {
        var supplier = await _serviceManager.PurchaseService.GetByPurchaseIDAsync(id, false, cancellationToken);
        return Ok(supplier);
    }

    [HttpPost(Name = "CreatePurchase")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Create([FromBody] PurchaseDto purchaseDto, CancellationToken cancellationToken)
    {
        var createdPurchase = await _serviceManager.PurchaseService.CreateAsync(purchaseDto, false, cancellationToken);
        return CreatedAtRoute("PurchaseByID", new { id = createdPurchase.PurchaseID }, createdPurchase);
    }

    [HttpPut(Name = "UpdatePurchase")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Update([FromBody] PurchaseDto purchaseDto, CancellationToken cancellationToken)
    {
        await _serviceManager.PurchaseService.UpdateAsync(purchaseDto, true, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "PartiallyUpdatePurchase")]
    public async Task<IActionResult> PartiallyUpdatePurchase(int id, [FromBody] JsonPatchDocument<PurchaseDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result = await _serviceManager.PurchaseService.GetPurchaseForPatchAsync(id, true);
        patchDoc.ApplyTo(result.purchaseToPatch);
        await _serviceManager.PurchaseService.SaveChangesForPatchAsync(result.purchaseToPatch, result.purchase);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeletePurchase")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _serviceManager.PurchaseService.DeleteAsync(id, false, cancellationToken);
        return NoContent();
    }
}
