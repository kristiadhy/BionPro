using Domain.DTO;
using Domain.Parameters;

namespace WebAssembly.StateManagement;

public class CustomerState
{
    //private readonly IServiceManager ServiceManager;
    public CustomerParam CustomerParameter { get; set; } = new();
    public CustomerDTO Customer { get; set; } = new();

    //For filters
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by customer name
    public string? FilterCustomerNameValue { get; set; }
    public bool IsFilterByCustomerNameActive { get; set; } = false;

    public void UpdateCustomerParametersBasedOnActiveFilters()
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            CustomerParameter.PageNumber = 1;

        if (IsFilterByCustomerNameActive)
        {
            CustomerParameter.SrcByName = FilterCustomerNameValue;
        }
        else
        {
            CustomerParameter.SrcByName = null;
        }
    }

    internal void ToggleFilterState()
    {
        IsFilterByCustomerNameActive = false;
        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterByCustomerNameActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
