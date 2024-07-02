using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class ProductState
{
    private readonly IServiceManager _serviceManager;
    public List<ProductDto> ProductList { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductParam ProductParameter { get; set; } = new();
    public ProductDto Product { get; set; } = new();

    public ProductState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadProductCategories()
    {
        var pagingResponse = await _serviceManager.ProductService.GetProductCategories(ProductParameter);
        ProductList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }
}
