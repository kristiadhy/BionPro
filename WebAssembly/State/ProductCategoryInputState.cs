using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.State;

public class ProductCategoryInputState
{
    public ProductCategoryDto ProductCategory { get; set; } = new();
}
