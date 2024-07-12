using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class PurchaseState
{
    private readonly IServiceManager _serviceManager;
    public List<PurchaseDtoForSummary> PurchaseListForSummary { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public PurchaseParam PurchaseParameter { get; set; } = new();
    public PurchaseDto PurchaseForTransaction { get; set; } = new();

    public PurchaseState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadPurchasesForSummary()
    {
        var pagingResponse = await _serviceManager.PurchaseService.GetPurchasesForSummary(PurchaseParameter);
        PurchaseListForSummary = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
