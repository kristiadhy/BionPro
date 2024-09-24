using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class ProductCategoryDropdownState(IServiceManager serviceManager)
{
    private readonly IServiceManager ServiceManager = serviceManager;

    public IEnumerable<ProductCategoryDto> ProductCategoryListDropdown { get; private set; } = [];

    public async Task LoadProductCategoriesDropDown()
    {
        ProductCategoryParam productCategoryParameter = new();
        var pagingResponse = await ServiceManager.ProductCategoryService.GetProductCategories(productCategoryParameter);
        ProductCategoryListDropdown = pagingResponse.Items;
    }

    public void SetProductCategoryDropdown(IEnumerable<ProductCategoryDto> productCategoryList)
    {
        ProductCategoryListDropdown = productCategoryList;
    }
}
