using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class ProductState
{
    private readonly IServiceManager _serviceManager;
    public List<ProductDtoForProductQueries> ProductList { get; set; } = [];
    public IEnumerable<ProductDtoForProductQueries> ProductListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductParam ProductParameter { get; set; } = new();
    public ProductDto Product { get; set; } = new();

    public ProductState(IServiceManager serviceManager)
    {
        _serviceManager = serviceManager;
    }

    public async Task LoadProducts()
    {
        var pagingResponse = await _serviceManager.ProductService.GetProducts(ProductParameter);
        ProductList = pagingResponse.Items;
        MetaData = pagingResponse.MetaData;
    }

    public async Task LoadProductsDropDown()
    {
        ProductParam productParameter = new();
        var pagingResponse = await _serviceManager.ProductService.GetProducts(productParameter);
        ProductListDropdown = pagingResponse.Items;
    }
}
