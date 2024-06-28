using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class ProductCategoryState
{
    private readonly IServiceManager _serviceManager;
    public List<ProductCategoryDto> ProductCategoryList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductCategoryParam ProductCategoryParameter { get; set; } = new();
    public ProductCategoryDto ProductCategory { get; set; } = new();

    public ProductCategoryState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadProductCategories()
    {
        var pagingResponse = await _serviceManager.ProductCategoryService.GetProductCategories(ProductCategoryParameter);
        ProductCategoryList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
