using Domain.Parameters;

namespace WebAssembly.State;

public class SupplierDisplayFilterState
{
    //For filters
    public bool IsFilterSet { get; set; } = false;
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by customer name
    public string? FilterSupplierNameValue { get; set; }
    public bool IsFilterBySupplierNameActive { get; set; } = false;

    public void UpdateSupplierParametersBasedOnActiveFilters(SupplierParam supplierParam)
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            supplierParam.PageNumber = 1;

        if (IsFilterBySupplierNameActive)
        {
            supplierParam.SrcByName = FilterSupplierNameValue;
        }
        else
        {
            supplierParam.SrcByName = null;
        }
    }

    internal void ToggleFilterState()
    {
        IsFilterBySupplierNameActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterBySupplierNameActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
