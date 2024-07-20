using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class CustomerState
{
    private IServiceManager ServiceManager;
    public List<CustomerDTO> CustomerList { get; set; } = [];
    public IEnumerable<CustomerDTO> CustomerListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public CustomerParam CustomerParameter { get; set; } = new();
    public CustomerDTO Customer { get; set; } = new();

    //For filters
    public bool IsFilterSet { get; set; } = false;
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by customer name
    public string? FilterCustomerNameValue { get; set; }
    public bool IsFilterByCustomerNameActive { get; set; } = false;
    //----------------------------------------------

    public CustomerState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadCustomers()
    {
        UpdateCustomerParametersBasedOnActiveFilters();
        var pagingResponse = await ServiceManager.CustomerService.GetCustomers(CustomerParameter);
        CustomerList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public async Task LoadCustomersDropDown()
    {
        CustomerParam supplierParameter = new();
        var pagingResponse = await ServiceManager.CustomerService.GetCustomers(supplierParameter);
        CustomerListDropdown = pagingResponse.Items;
    }

    private void UpdateCustomerParametersBasedOnActiveFilters()
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

    public void ResetCustomerData()
    {
        CustomerList = [];
        MetaData = new();
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
