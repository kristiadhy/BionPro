using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface ISaleHttpService
{
  public Task<PagingResponse<SaleDtoForSummary>> GetSalesForSummary(SaleParam saleParam);
  public Task<SaleDto> GetSaleByID(int saleID);
  public Task Create(SaleDto saleDto);
  public Task Update(SaleDto saleDto);
  public Task Delete(int saleID);
}
