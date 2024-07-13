using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class PurchaseState
{
    private readonly IServiceManager ServiceManager;
    public List<PurchaseDtoForSummary> PurchaseListForSummary { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public PurchaseParam PurchaseParameter { get; set; } = new();
    public PurchaseDto PurchaseForTransaction { get; set; } = new();

    public PurchaseState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadPurchasesForSummary()
    {
        var pagingResponse = await ServiceManager.PurchaseService.GetPurchasesForSummary(PurchaseParameter);
        PurchaseListForSummary = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
