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

    //For filters
    public bool IsFilterSet { get; set; } = false;
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by transaction date
    public TimePeriod? FilterDateParentValue { get; set; }
    public Enum? FilterDateDetailValue { get; set; }
    public DateTime? FilterDateStartDate { get; set; }
    public DateTime? FilterDateEndDate { get; set; }
    //We don't use auto property this time because we want to trigger the ResetPurchaseData() method when the value is changed
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
    //----------------------------------------------

    //Set for filter by supplier
    public Guid? FilterSupplierByID { get; set; }
    public string? FilterSupplierByName { get; set; }
    public bool IsFilterBySupplierActive { get; set; } = false;
    //----------------------------------------------

    public PurchaseState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadPurchasesForSummary()
    {
        UpdatePurchaseParametersBasedOnActiveFilters();
        var pagingResponse = await ServiceManager.PurchaseService.GetPurchasesForSummary(PurchaseParameter);
        PurchaseListForSummary = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    private void UpdatePurchaseParametersBasedOnActiveFilters()
    {
        if (_isFilterByDateActive)
        {
            PurchaseParameter.SrcDateFrom = FilterDateStartDate;
            PurchaseParameter.SrcDateTo = FilterDateEndDate;
        }
        else
        {
            PurchaseParameter.SrcDateFrom = null;
            PurchaseParameter.SrcDateTo = null;
        }

        if (IsFilterBySupplierActive)
        {
            PurchaseParameter.SrcSupplierID = FilterSupplierByID;
            PurchaseParameter.SrcSupplierName = FilterSupplierByName;
        }
        else
        {
            PurchaseParameter.SrcSupplierID = null;
            PurchaseParameter.SrcSupplierName = null;
        }
    }

    public void ResetPurchaseData()
    {
        PurchaseListForSummary = [];
        MetaData = new();
    }

    internal void ToggleFilterState()
    {
        IsFilterBySupplierActive = false;
        IsFilterByDateActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if(IsFilterBySupplierActive || IsFilterByDateActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
