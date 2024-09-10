using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IPurchaseHttpService
{
    public Task<PagingResponse<PurchaseDtoForSummary>> GetPurchasesForSummary(PurchaseParam purchaseParam);
    public Task<PurchaseDto> GetPurchaseByID(int purchaseID);
    public Task<HttpResponseMessage> Create(PurchaseDto purchaseDto);
    public Task<HttpResponseMessage> Update(PurchaseDto purchaseDto);
    public Task<HttpResponseMessage> Delete(int purchaseID);
}
