using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class ProductCategoryDisplayState(IServiceManager serviceManager)
{
    private readonly IServiceManager ServiceManager = serviceManager;

    [Inject]
    ProductCategoryDropdownState ProductCategoryDropdownState { get; set; } = default!;

    public List<ProductCategoryDto> ProductCategoryList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductCategoryParam ProductCategoryParameter { get; set; } = new();

    public async Task LoadProductCategories()
    {
        var pagingResponse = await ServiceManager.ProductCategoryService.GetProductCategories(ProductCategoryParameter);
        ProductCategoryList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
        CopyProductCategoryToDropDown();
    }

    private void CopyProductCategoryToDropDown()
    {
        ProductCategoryDropdownState.SetProductCategoryDropdown(ProductCategoryList);
    }
}
