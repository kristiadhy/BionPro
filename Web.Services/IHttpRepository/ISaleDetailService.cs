using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface ISaleDetailService
{
    public Task<PagingResponse<SaleDetailDto>> GetSaleByID(int saleID, SaleDetailParam saleDetailParam);
}
