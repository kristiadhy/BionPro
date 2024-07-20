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
[Route("api/sales")]
public class SaleController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;

    [HttpGet(Name = "Sales")]
    public async Task<IActionResult> GetSummaryByParameters(int saleID, [FromQuery] SaleParam saleParam, CancellationToken cancellationToken)
    {
        var pagedResult = await _serviceManager.SaleService.GetSummaryByParametersAsync(saleID, saleParam, false, cancellationToken);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
        return Ok(pagedResult.saleDto);
    }


    [HttpGet("{id:int}", Name = "SaleByID")]
    public async Task<IActionResult> GetByID(int id, CancellationToken cancellationToken)
    {
        var sales = await _serviceManager.SaleService.GetBySaleIDAsync(id, false, cancellationToken);
        return Ok(sales);
    }

    [HttpPost(Name = "CreateSale")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Create([FromBody] SaleDto saleDto, CancellationToken cancellationToken)
    {
        var createdSale = await _serviceManager.SaleService.CreateAsync(saleDto, false, cancellationToken);
        return CreatedAtRoute("SaleByID", new { id = createdSale.SaleID }, createdSale);
    }

    [HttpPut(Name = "UpdateSale")]
    [ServiceFilter(typeof(ValidationFilterAttribute))]
    public async Task<IActionResult> Update([FromBody] SaleDto saleDto, CancellationToken cancellationToken)
    {
        await _serviceManager.SaleService.UpdateAsync(saleDto, true, cancellationToken);
        return NoContent();
    }

    [HttpPatch("{id:int}", Name = "PartiallyUpdateSale")]
    public async Task<IActionResult> PartiallyUpdateSale(int id, [FromBody] JsonPatchDocument<SaleDto> patchDoc)
    {
        if (patchDoc is null)
            return BadRequest("patchDoc object sent from client is null.");

        var result = await _serviceManager.SaleService.GetSaleForPatchAsync(id, true);
        patchDoc.ApplyTo(result.saleToPatch);
        await _serviceManager.SaleService.SaveChangesForPatchAsync(result.saleToPatch, result.sale);
        return NoContent();
    }

    [HttpDelete("{id:int}", Name = "DeleteSale")]
    public async Task<IActionResult> Delete(int id, CancellationToken cancellationToken)
    {
        await _serviceManager.SaleService.DeleteAsync(id, false, cancellationToken);
        return NoContent();
    }
}
