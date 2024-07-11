using WebAssembly.Model;

namespace WebAssembly.Constants;

public static class GlobalConstant
{
    public static string AppName = "BionPro";
    public static string LoggedUser = "Admin";
    public static IEnumerable<PageModel> PageModels = new List<PageModel>
    {
        new() { ID =1, Title ="Customers", Path="/customers" },
        new() { ID =2, Title ="Suppliers", Path="/suppliers" },
        new() { ID =3, Title ="Products", Path="/products" },
        new() { ID =4, Title ="Product Categories", Path="/products/categories" },
        new() { ID =5, Title ="Purchases", Path="/purchases" },
    };
}
