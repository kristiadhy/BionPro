using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class PurchaseDetailState
{
    private readonly IServiceManager _serviceManager;
    public List<PurchaseDetailDto> PurchaseDetailList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    //public PurchaseDetailParam PurchaseDetailParameter { get; set; } = new();
    public PurchaseDetailDto PurchaseDetail { get; set; } = new();

    public PurchaseDetailState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadPurchaseDetails()
    {
        //var pagingResponse = await _serviceManager.PurchaseDetailService.GetPurchaseDetails(PurchaseDetailParameter);
        //PurchaseDetailList = pagingResponse.Items;
        //MetaData = pagingResponse.MetaData;
    }
}
