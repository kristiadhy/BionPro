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
[Route("api/purchase/details")]
public class PurchseDetailController(IServiceManager serviceManager) : ControllerBase
{
    private readonly IServiceManager _serviceManager = serviceManager;


    [HttpGet("{id:int}", Name = "PurchaseDetailByID")]
    public async Task<IActionResult> GetByID(int id, [FromQuery] PurchaseDetailParam purchaseDetailParam, CancellationToken cancellationToken)
    {
        var pagedResult = await _serviceManager.PurchaseDetailService.GetByPurchaseDetailByIDAsync(id, purchaseDetailParam, false, cancellationToken);
        Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
        return Ok(pagedResult.purchaseDetailDto);
    }
}
