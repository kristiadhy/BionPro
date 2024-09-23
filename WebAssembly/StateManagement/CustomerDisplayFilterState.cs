using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class CustomerDisplayFilterState()
{
    //For filters
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by customer name
    public string? FilterCustomerNameValue { get; set; }
    public bool IsFilterByCustomerNameActive { get; set; } = false;

    public void UpdateCustomerParametersBasedOnActiveFilters(CustomerParam customerParam)
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            customerParam.PageNumber = 1;

        if (IsFilterByCustomerNameActive)
        {
            customerParam.SrcByName = FilterCustomerNameValue;
        }
        else
        {
            customerParam.SrcByName = null;
        }
    }

    public void ToggleFilterState()
    {
        IsFilterByCustomerNameActive = false;
        IsFilterActive = false;
    }

    public void SetGlobalFilterStateByFilters()
    {
        if (IsFilterByCustomerNameActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
