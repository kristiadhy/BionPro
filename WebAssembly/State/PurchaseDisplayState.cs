using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class PurchaseDisplayState(IServiceManager serviceManager, PurchaseDisplayFilterState purchaseDisplayFilterState)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly PurchaseDisplayFilterState _purchaseDisplayFilterState = purchaseDisplayFilterState;

    public List<PurchaseDtoForSummary> PurchaseListForSummary { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public PurchaseParam PurchaseParameter { get; set; } = new();


    public async Task LoadPurchasesForSummary()
    {
        _purchaseDisplayFilterState.UpdatePurchaseParametersBasedOnActiveFilters(PurchaseParameter);
        var pagingResponse = await _serviceManager.PurchaseService.GetPurchasesForSummary(PurchaseParameter);
        PurchaseListForSummary = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public void ResetPurchaseData()
    {
        PurchaseListForSummary = [];
        MetaData = new();
    }
}
