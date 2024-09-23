using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class SupplierState
{
    private readonly IServiceManager _serviceManager;
    public List<SupplierDto> SupplierList { get; set; } = [];
    public IEnumerable<SupplierDto> SupplierListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SupplierParam SupplierParameter { get; set; } = new();
    public SupplierDto Supplier { get; set; } = new();

    //For filters
    public bool IsFilterSet { get; set; } = false;
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by customer name
    public string? FilterSupplierNameValue { get; set; }
    public bool IsFilterBySupplierNameActive { get; set; } = false;
    //----------------------------------------------

    public SupplierState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadSuppliers()
    {
        UpdateSupplierParametersBasedOnActiveFilters();
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(SupplierParameter);
        SupplierList = pagingResponse.Items ?? [];
        MetaData = pagingResponse.MetaData ?? new();
    }

    public async Task LoadSuppliersDropDown()
    {
        SupplierParam supplierParameter = new();
        var pagingResponse = await _serviceManager.SupplierService.GetSuppliers(supplierParameter);
        SupplierListDropdown = pagingResponse.Items ?? [];
    }

    private void UpdateSupplierParametersBasedOnActiveFilters()
    {
        if (IsFilterActive)
            //Always set the page to 1 when filters are active
            SupplierParameter.PageNumber = 1;

        if (IsFilterBySupplierNameActive)
        {
            SupplierParameter.SrcByName = FilterSupplierNameValue;
        }
        else
        {
            SupplierParameter.SrcByName = null;
        }
    }

    public void ResetSupplierData()
    {
        SupplierList = [];
        MetaData = new();
    }

    internal void ToggleFilterState()
    {
        IsFilterBySupplierNameActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterBySupplierNameActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
