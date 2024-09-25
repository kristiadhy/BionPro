using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class SupplierDisplayState(IServiceManager serviceManager, SupplierDisplayFilterState supplierDisplayFilterState)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly SupplierDisplayFilterState _supplierDisplayFilterState = supplierDisplayFilterState;
    public List<SupplierDto> SupplierList { get; set; } = [];
    public IEnumerable<SupplierDto> SupplierListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SupplierParam SupplierParameter { get; set; } = new();
    public SupplierDto Supplier { get; set; } = new();

    public async Task LoadSuppliers()
    {
        _supplierDisplayFilterState.UpdateSupplierParametersBasedOnActiveFilters(SupplierParameter);
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(SupplierParameter);
        SupplierList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public void ResetSupplierData()
    {
        SupplierList = [];
        MetaData = new();
    }
}
