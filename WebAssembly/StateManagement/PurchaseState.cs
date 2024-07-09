using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class PurchaseState
{
    private readonly IServiceManager _serviceManager;
    public List<PurchaseDtoForQueries> PurchaseList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public PurchaseParam PurchaseParameter { get; set; } = new();
    public PurchaseDto Purchase { get; set; } = new();
    public PurchaseDetailDto PurchaseDetailForTransaction { get; set; } = new();
    public List<PurchaseDetailDto> PurchaseDetailListForTransaction { get; set; } = [];

    public PurchaseState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadPurchases()
    {
        var pagingResponse = await _serviceManager.PurchaseService.GetPurchases(PurchaseParameter);
        PurchaseList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
