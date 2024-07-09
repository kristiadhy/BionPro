using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class SupplierState
{
    private IServiceManager _serviceManager;
    public List<SupplierDto> SupplierList { get; set; } = [];
    public IEnumerable<SupplierDto> SupplierListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SupplierParam SupplierParameter { get; set; } = new();
    public SupplierDto Supplier { get; set; } = new();

    public SupplierState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadSuppliers()
    {
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(SupplierParameter);
        SupplierList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public async Task LoadSuppliersDropDown()
    {
        SupplierParam supplierParameter = new();
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(supplierParameter);
        SupplierListDropdown = pagingResponse.Items;
    }
}
