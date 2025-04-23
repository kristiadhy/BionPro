using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IPurchaseHttpService
{
  public Task<PagingResponse<PurchaseDtoForSummary>> GetPurchasesForSummary(PurchaseParam purchaseParam);
  public Task<PurchaseDto> GetPurchaseByID(int purchaseID);
  public Task Create(PurchaseDto purchaseDto);
  public Task Update(PurchaseDto purchaseDto);
  public Task Delete(int purchaseID);
}
