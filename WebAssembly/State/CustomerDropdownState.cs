using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class CustomerDropdownState(IServiceManager serviceManager)
{
    private readonly IServiceManager ServiceManager = serviceManager;
    public IEnumerable<CustomerDTO> CustomerListDropdown { get; set; } = [];

    public async Task LoadCustomersDropDown()
    {
        CustomerParam supplierParameter = new();
        var pagingResponse = await ServiceManager.CustomerService.GetCustomers(supplierParameter);
        CustomerListDropdown = pagingResponse.Items ?? [];
    }
}
