using WebAssembly.Model;

namespace WebAssembly.StateManagement;

public static class GlobalState
{
    public static string AppName = "BionPro";
    public static string LoggedUser = "Admin";
    public static IEnumerable<PageModel> PageModels = new List<PageModel>
    {
        new PageModel { ID =1, Title ="Customers", Path="/customers" },
        new PageModel { ID =2, Title ="Suppliers", Path="/suppliers" },
        new PageModel { ID =3, Title ="Products", Path="/products" },
        new PageModel { ID =4, Title ="Product Categories", Path="/products/categories" },
    };
}
