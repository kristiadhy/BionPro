using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IPurchaseDetailHttpService
{
    public Task<DataResponse<PurchaseDetailDto>> GetPurchaseByID(int purchaseID, PurchaseDetailParam purchaseDetailParam);
}
