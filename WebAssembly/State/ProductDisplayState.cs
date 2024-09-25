using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class ProductDisplayState(IServiceManager serviceManager, ProductDisplayFilterState productDisplayFilterState, ProductDropdownState productDropdownState)
{
    private readonly IServiceManager ServiceManager = serviceManager;
    private readonly ProductDisplayFilterState _productDisplayFilterState = productDisplayFilterState;
    private readonly ProductDropdownState _productDropdownState = productDropdownState;

    public List<ProductDtoForProductQueries> ProductList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductParam ProductParameter { get; set; } = new();

    public async Task LoadProducts()
    {
        _productDisplayFilterState.UpdateProductParametersBasedOnActiveFilters(ProductParameter);
        var pagingResponse = await ServiceManager.ProductService.GetProducts(ProductParameter);
        ProductList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
        CopyProductsToDropDown();
    }

    private void CopyProductsToDropDown()
    {
        _productDropdownState.SetProductsDropdown(ProductList);
    }

    public void ResetPurchaseData()
    {
        ProductList = [];
        MetaData = new();
    }
}
