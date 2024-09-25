using Domain.DTO;

namespace WebAssembly.State;

public class ProductCategoryInputState
{
    public ProductCategoryDto ProductCategory { get; set; } = new();
}
