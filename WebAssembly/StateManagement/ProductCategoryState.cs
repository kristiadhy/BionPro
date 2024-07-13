using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class ProductCategoryState
{
    private readonly IServiceManager ServiceManager;
    public List<ProductCategoryDto> ProductCategoryList { get; set; } = [];
    public IEnumerable<ProductCategoryDto> ProductCategoryListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductCategoryParam ProductCategoryParameter { get; set; } = new();
    public ProductCategoryDto ProductCategory { get; set; } = new();

    public ProductCategoryState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadProductCategories()
    {
        var pagingResponse = await ServiceManager.ProductCategoryService.GetProductCategories(ProductCategoryParameter);
        ProductCategoryList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
        CopyProductCategoryToDropDown();
    }

    public async Task LoadProductCategoriesDropDown()
    {
        ProductCategoryParam productCategoryParameter = new();
        var pagingResponse = await ServiceManager.ProductCategoryService.GetProductCategories(productCategoryParameter);
        ProductCategoryListDropdown = pagingResponse.Items;
    }

    public void CopyProductCategoryToDropDown()
    {
        ProductCategoryListDropdown = ProductCategoryList;
    }
}
