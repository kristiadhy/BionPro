using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface ISaleDetailHttpService
{
    public Task<PagingResponse<SaleDetailDto>> GetSaleByID(int saleID, SaleDetailParam saleDetailParam);
}
