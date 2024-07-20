using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface ISaleService
{
    public Task<PagingResponse<SaleDtoForSummary>> GetSalesForSummary(SaleParam saleParam);
    public Task<SaleDto> GetSaleByID(int saleID);
    public Task<HttpResponseMessage> Create(SaleDto saleDto);
    public Task<HttpResponseMessage> Update(SaleDto saleDto);
    public Task<HttpResponseMessage> Delete(int saleID);
}
