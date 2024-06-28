using WebAssembly.Model;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class ProductDisplay
{
    private PageModel? ProductsPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }

    public ProductDisplay()
    {
        ProductsPageModel = GlobalState.PageModels.Where(s => s.ID == 3).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = ProductsPageModel?.Path, Title= ProductsPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    protected async Task EvReloadData()
    {

    }

    protected async Task EvLoadData()
    {

    }

    protected async Task EvCreateNew()
    {

    }
}
