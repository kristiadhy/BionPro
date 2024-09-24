using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class CustomerDisplayState(IServiceManager serviceManager, CustomerDisplayFilterState customerDisplayFilterState)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly CustomerDisplayFilterState _customerDisplayFilterState = customerDisplayFilterState;

    public CustomerParam CustomerParameter { get; set; } = new();
    public List<CustomerDTO> CustomerList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();

    public async Task LoadCustomers()
    {
        _customerDisplayFilterState.UpdateCustomerParametersBasedOnActiveFilters(CustomerParameter);
        var pagingResponse = await _serviceManager.CustomerService.GetCustomers(CustomerParameter);
        CustomerList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
