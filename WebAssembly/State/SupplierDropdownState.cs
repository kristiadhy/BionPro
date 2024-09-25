using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class SupplierDropdownState(IServiceManager serviceManager)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    public IEnumerable<SupplierDto> SupplierListDropdown { get; set; } = [];

    public async Task LoadSuppliersDropDown()
    {
        SupplierParam supplierParameter = new();
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(supplierParameter);
        SupplierListDropdown = pagingResponse.Items ?? [];
    }
}
