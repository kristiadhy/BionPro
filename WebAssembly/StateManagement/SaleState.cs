using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;
using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.StateManagement;

public class SaleState
{
    private readonly IServiceManager ServiceManager;

    public List<SaleDtoForSummary> SaleListForSummary { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SaleParam SaleParameter { get; set; } = new();

    public SaleDto SaleForTransaction { get; set; } = new();

    //For filters
    public bool IsFilterSet { get; set; } = false;
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by transaction date
    public TimePeriod? FilterDateParentValue { get; set; }
    public Enum? FilterDateDetailValue { get; set; }
    public DateTime? FilterDateStartDate { get; set; }
    public DateTime? FilterDateEndDate { get; set; }
    //We don't use auto property this time because we want to trigger the ResetSaleData() method when the value is changed
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
            //    ResetSaleData();
            //}
        }
    }
    //----------------------------------------------

    //Set for filter by supplier
    public Guid? FilterCustomerByID { get; set; }
    public string? FilterCustomerByName { get; set; }
    public bool IsFilterByCustomerActive { get; set; } = false;
    //----------------------------------------------

    public SaleState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadSalesForSummary()
    {
        UpdateSaleParametersBasedOnActiveFilters();
        var pagingResponse = await ServiceManager.SaleService.GetSalesForSummary(SaleParameter);
        SaleListForSummary = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    private void UpdateSaleParametersBasedOnActiveFilters()
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            SaleParameter.PageNumber = 1;

        if (_isFilterByDateActive)
        {
            SaleParameter.SrcDateFrom = FilterDateStartDate;
            SaleParameter.SrcDateTo = FilterDateEndDate;
        }
        else
        {
            SaleParameter.SrcDateFrom = null;
            SaleParameter.SrcDateTo = null;
        }

        if (IsFilterByCustomerActive)
        {
            SaleParameter.SrcCustomerID = FilterCustomerByID;
            SaleParameter.SrcCustomerName = FilterCustomerByName;
        }
        else
        {
            SaleParameter.SrcCustomerID = null;
            SaleParameter.SrcCustomerName = null;
        }
    }

    public void ResetSaleData()
    {
        SaleListForSummary = [];
        MetaData = new();
    }

    internal void ToggleFilterState()
    {
        IsFilterByCustomerActive = false;
        IsFilterByDateActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterByCustomerActive || IsFilterByDateActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
