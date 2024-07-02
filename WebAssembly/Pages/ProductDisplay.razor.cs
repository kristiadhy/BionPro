using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class ProductDisplay
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    ProductState ProductState { get; set; } = default!;

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

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{ProductsPageModel?.Path}/create");
    }
}
