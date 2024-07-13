using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class ProductState
{
    private readonly IServiceManager ServiceManager;
    public List<ProductDtoForProductQueries> ProductList { get; set; } = [];
    public IEnumerable<ProductDtoForProductQueries> ProductListDropdown { get; set; } = [];
    public MetaData MetaData { get; set; } = new();
    public ProductParam ProductParameter { get; set; } = new();
    public ProductDto Product { get; set; } = new();

    public ProductState(IServiceManager serviceManager)
    {
        ServiceManager = serviceManager;
    }

    public async Task LoadProducts()
    {
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
}
