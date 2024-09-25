using Domain.Parameters;

namespace WebAssembly.State;

public class ProductDisplayFilterState
{
    public bool IsFilterActive { get; set; } = false;

    public int? FilterProductCategoryValue { get; set; }
    public bool IsFilterByProductCategoryActive { get; set; } = false;
    public string? FilterProductNameValue { get; set; }
    public bool IsFilterByProductNameActive { get; set; } = false;


    public void UpdateProductParametersBasedOnActiveFilters(ProductParam productParam)
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            productParam.PageNumber = 1;

        if (IsFilterByProductCategoryActive)
        {
            productParam.SrcByProductCategory = FilterProductCategoryValue;
        }
        else
        {
            productParam.SrcByProductCategory = null;
        }

        if (IsFilterByProductNameActive)
        {
            productParam.SrcByProductName = FilterProductNameValue;
        }
        else
        {
            productParam.SrcByProductName = null;
        }
    }

    internal void ToggleFilterState()
    {
        IsFilterByProductCategoryActive = false;
        IsFilterByProductNameActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterByProductCategoryActive || IsFilterByProductNameActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
