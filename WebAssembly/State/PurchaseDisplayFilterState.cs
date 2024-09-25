using Domain.Parameters;
using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.State;

public class PurchaseDisplayFilterState()
{
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


    public void UpdatePurchaseParametersBasedOnActiveFilters(PurchaseParam purchaseParam)
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            purchaseParam.PageNumber = 1;

        if (_isFilterByDateActive)
        {
            purchaseParam.SrcDateFrom = FilterDateStartDate;
            purchaseParam.SrcDateTo = FilterDateEndDate;
        }
        else
        {
            purchaseParam.SrcDateFrom = null;
            purchaseParam.SrcDateTo = null;
        }

        if (IsFilterBySupplierActive)
        {
            purchaseParam.SrcSupplierID = FilterSupplierByID;
            purchaseParam.SrcSupplierName = FilterSupplierByName;
        }
        else
        {
            purchaseParam.SrcSupplierID = null;
            purchaseParam.SrcSupplierName = null;
        }
    }

    internal void ToggleFilterState()
    {
        IsFilterBySupplierActive = false;
        IsFilterByDateActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterBySupplierActive || IsFilterByDateActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
