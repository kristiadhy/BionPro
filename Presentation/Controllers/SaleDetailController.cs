using Asp.Versioning;
using Domain.Parameters;
using Microsoft.AspNetCore.Mvc;
using Services.Contracts;
using System.Text.Json;

namespace Presentation.Controllers;
[ApiVersion("1.0")]
[ApiController]
[Route("api/sale/details")]
public class SaleDetailController(IServiceManager serviceManager) : ControllerBase
{
  private readonly IServiceManager _serviceManager = serviceManager;


  [HttpGet("{id:int}", Name = "SaleDetailByID")]
  public async Task<IActionResult> GetByID(int id, [FromQuery] SaleDetailParam saleDetailParam, CancellationToken cancellationToken)
  {
    var pagedResult = await _serviceManager.SaleDetailService.GetBySaleDetailByIDAsync(id, saleDetailParam, false, cancellationToken);
    Response.Headers["X-Pagination"] = JsonSerializer.Serialize(pagedResult.metaData);
    return Ok(pagedResult.saleDetailDto);
  }
}
