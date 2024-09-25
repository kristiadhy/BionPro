using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class ProductDropdownState(IServiceManager serviceManager)
{
    private readonly IServiceManager ServiceManager = serviceManager;
    public IEnumerable<ProductDtoForProductQueries> ProductListDropdown { get; set; } = [];

    public async Task LoadProductsDropDown()
    {
        ProductParam productParameter = new();
        var pagingResponse = await ServiceManager.ProductService.GetProducts(productParameter);
        ProductListDropdown = pagingResponse.Items ?? [];
    }

    public void SetProductsDropdown(IEnumerable<ProductDtoForProductQueries> productDto)
    {
        ProductListDropdown = productDto;
    }
}
