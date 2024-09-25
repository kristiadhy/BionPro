using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class PurchaseDetailState(IServiceManager serviceManager)
{
    private readonly IServiceManager ServiceManager = serviceManager;
    public List<PurchaseDetailDto> PurchaseDetailList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public PurchaseDetailDto PurchaseDetail { get; set; } = new();

    //public async Task LoadPurchaseDetails()
    //{
    //    //var pagingResponse = await ServiceManager.PurchaseDetailService.GetPurchaseDetails(PurchaseDetailParameter);
    //    //PurchaseDetailList = pagingResponse.Items;
    //    //MetaData = pagingResponse.MetaData;
    //}
}
