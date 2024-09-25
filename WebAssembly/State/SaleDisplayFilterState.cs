using Domain.Parameters;
using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.State;

public class SaleDisplayFilterState
{
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

    public void UpdateSaleParametersBasedOnActiveFilters(SaleParam saleParam)
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            saleParam.PageNumber = 1;

        if (_isFilterByDateActive)
        {
            saleParam.SrcDateFrom = FilterDateStartDate;
            saleParam.SrcDateTo = FilterDateEndDate;
        }
        else
        {
            saleParam.SrcDateFrom = null;
            saleParam.SrcDateTo = null;
        }

        if (IsFilterByCustomerActive)
        {
            saleParam.SrcCustomerID = FilterCustomerByID;
            saleParam.SrcCustomerName = FilterCustomerByName;
        }
        else
        {
            saleParam.SrcCustomerID = null;
            saleParam.SrcCustomerName = null;
        }
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
