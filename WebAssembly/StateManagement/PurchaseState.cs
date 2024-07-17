using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;
using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.StateManagement;

public class PurchaseState
{
    private readonly IServiceManager ServiceManager;
    public List<PurchaseDtoForSummary> PurchaseListForSummary { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public PurchaseParam PurchaseParameter { get; set; } = new();
    public PurchaseDto PurchaseForTransaction { get; set; } = new();

    //Set transaction date filter for purchase display
    public TimePeriod? FilterDateParentValue { get; set; }
    public Enum? FilterDateDetailValue { get; set; }
    public DateTime? FilterDateStartDate { get; set; }
    public DateTime? FilterDateEndDate { get; set; }
    public bool IsFilterSet { get; set; } = false;

    private bool _isFilterByDateActive = false;
    public bool IsFilterByDateActive
    {
        get => _isFilterByDateActive;
        set
        {
            if (_isFilterByDateActive != value)
            {
                _isFilterByDateActive = value;
            }

            //if (!_isFilterByDateActive)
            //{
            //    ResetPurchaseData();
            //}
        }
    }

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

    public void ResetPurchaseData()
    {
        PurchaseListForSummary = [];
        MetaData = new();
    }
}
