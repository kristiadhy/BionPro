using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;
using static WebAssembly.Shared.Enum.DataFilterEnum;

namespace WebAssembly.StateManagement;

public class ProductState
{
    private readonly IServiceManager ServiceManager;
    public List<ProductDtoForProductQueries> ProductList { get; set; } = [];
    public IEnumerable<ProductDtoForProductQueries> ProductListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductParam ProductParameter { get; set; } = new();
    public ProductDto Product { get; set; } = new();

    //For filters
    public bool IsFilterSet { get; set; } = false;
    public bool IsFilterActive { get; set; } = false;

    //Set for filter by product category
    public int? FilterProductCategoryValue { get; set; }
    public bool IsFilterByProductCategoryActive { get; set; } = false;
    //----------------------------------------------

    //Set for filter by product name
    public string? FilterProductNameValue { get; set; }
    public bool IsFilterByProductNameActive { get; set; } = false;
    //----------------------------------------------

    public ProductState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadProducts()
    {
        UpdateProductParametersBasedOnActiveFilters();
        var pagingResponse = await ServiceManager.ProductService.GetProducts(ProductParameter);
        ProductList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public async Task LoadProductsDropDown()
    {
        ProductParam productParameter = new();
        var pagingResponse = await ServiceManager.ProductService.GetProducts(productParameter);
        ProductListDropdown = pagingResponse.Items;
    }

    private void UpdateProductParametersBasedOnActiveFilters()
    {
        if (IsFilterByProductCategoryActive)
        {
            ProductParameter.SrcByProductCategory = FilterProductCategoryValue;
        }
        else
        {
            ProductParameter.SrcByProductCategory = null;
        }

        if (IsFilterByProductNameActive)
        {
            ProductParameter.SrcByProductName = FilterProductNameValue;
        }
        else
        {
            ProductParameter.SrcByProductName = null;
        }
    }

    public void ResetPurchaseData()
    {
        ProductList = [];
        MetaData = new();
    }

    internal void ToggleFilterState()
    {
        IsFilterByProductCategoryActive = false;
        IsFilterByProductNameActive = false;

        IsFilterActive = false;
    }

    internal void SetGlobalFilterStateByFilters()
    {
        if (IsFilterByProductCategoryActive || IsFilterByProductNameActive)
            IsFilterActive = true;
        else
            IsFilterActive = false;
    }
}
