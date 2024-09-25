using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class SaleDisplayState(IServiceManager serviceManager, SaleDisplayFilterState saleDisplayFilterState)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly SaleDisplayFilterState _saleDisplayFilterState = saleDisplayFilterState;

    public List<SaleDtoForSummary> SaleListForSummary { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public SaleParam SaleParameter { get; set; } = new();

    public async Task LoadSalesForSummary()
    {
        _saleDisplayFilterState.UpdateSaleParametersBasedOnActiveFilters(SaleParameter);
        var pagingResponse = await _serviceManager.SaleService.GetSalesForSummary(SaleParameter);
        SaleListForSummary = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public void ResetSaleData()
    {
        SaleListForSummary = [];
        MetaData = new();
    }
}
