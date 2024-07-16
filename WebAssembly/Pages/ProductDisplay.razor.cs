using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Constants;
using WebAssembly.CustomEventArgs;
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

    internal static RadzenDataGrid<ProductDtoForProductQueries> ProductGrid { get; set; } = default!;

    private bool isLoading = false;

    private PageModel? ProductsPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }

    public ProductDisplay()
    {
        ProductsPageModel = GlobalConstant.PageModels.Where(s => s.ID == 3).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = ProductsPageModel?.Path, Title= ProductsPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    private async Task EvReloadData()
    {
        await EvLoadData();
        await ProductGrid.Reload();
    }

    private async Task EvLoadData()
    {
        isLoading = true;
        await ProductState.LoadProducts();
        isLoading = false;
    }

    private void EvEditRow(ProductDtoForProductQueries products)
    {
        NavigationManager.NavigateTo($"{ProductsPageModel?.Path}/edit/{products.ProductID}");
    }

    private async Task EvDeleteRow(ProductDtoForProductQueries products)
    {
        if (products is null)
            return;

        string productName = products.Name ?? string.Empty;
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Product", productName);
        if (!confirmationStatus)
            return;

        Guid productID = (Guid)products.ProductID!;
        if (products.ImageUrl is not null)
            await ServiceManager.ProductService.DeleteProductImage(products.ImageUrl);

        var response = await ServiceManager.ProductService.Delete(productID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Product has been deleted");
        await EvReloadData();
    }

    private void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{ProductsPageModel?.Path}/create");
    }

    private async Task PageChanged(PagerOnChangedEventArgs args)
    {
        ProductState.ProductParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }
}
