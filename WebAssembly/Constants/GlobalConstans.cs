namespace WebAssembly.Constants;

//TODO: Set appliication name as a global constant
public static class GlobalConstant
{
    public static readonly IEnumerable<PageModel> PageModels =
    [
        new PageModel { ID = 1, Title = "Customers", Path = "/customers" },
        new PageModel { ID = 2, Title = "Suppliers", Path = "/suppliers" },
        new PageModel { ID = 3, Title = "Products", Path = "/products" },
        new PageModel { ID = 4, Title = "Product Categories", Path = "/products/categories" },
        new PageModel { ID = 5, Title = "Purchases", Path = "/purchases" },
        new PageModel { ID = 6, Title = "Sales", Path = "/sales" },
    ];
}
