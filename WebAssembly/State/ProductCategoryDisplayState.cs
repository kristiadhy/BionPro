using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class ProductCategoryDisplayState(IServiceManager serviceManager, ProductCategoryDropdownState productCategoryDropdownState)
{
    private readonly IServiceManager _serviceManager = serviceManager;
    private readonly ProductCategoryDropdownState _productCategoryDropdownState = productCategoryDropdownState;

    public List<ProductCategoryDto> ProductCategoryList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductCategoryParam ProductCategoryParameter { get; set; } = new();

    public async Task LoadProductCategories()
    {
        var pagingResponse = await _serviceManager.ProductCategoryService.GetProductCategories(ProductCategoryParameter);
        ProductCategoryList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
        CopyProductCategoryToDropDown();
    }

    private void CopyProductCategoryToDropDown()
    {
        _productCategoryDropdownState.SetProductCategoryDropdown(ProductCategoryList);
    }
}
