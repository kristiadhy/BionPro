using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class SupplierState
{
    private IServiceManager ServiceManager;
    public List<SupplierDto> SupplierList { get; set; } = [];
    public IEnumerable<SupplierDto> SupplierListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SupplierParam SupplierParameter { get; set; } = new();
    public SupplierDto Supplier { get; set; } = new();

    public SupplierState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadSuppliers()
    {
        var pagingResponse = await ServiceManager.SupplierService.GetSuppliers(SupplierParameter);
        SupplierList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public async Task LoadSuppliersDropDown()
    {
        SupplierParam supplierParameter = new();
        var pagingResponse = await ServiceManager.SupplierService.GetSuppliers(supplierParameter);
        SupplierListDropdown = pagingResponse.Items;
    }
}
