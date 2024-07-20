using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.CustomEventArgs;

namespace WebAssembly.Pages;

public partial class ProductCategoryDisplay
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    CustomTooltipService CustomTooltipService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    ProductCategoryState ProductCategoryState { get; set; } = default!;

    internal static RadzenDataGrid<ProductCategoryDto> ProductCategoryGrid { get; set; } = default!;

    private bool isLoading = false;
    private PageModel? ProductCategoryPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }

    public ProductCategoryDisplay()
    {
        ProductCategoryPageModel = GlobalConstant.PageModels.Where(s => s.ID == 4).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = ProductCategoryPageModel?.Path, Title= ProductCategoryPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    protected async Task EvReloadData()
    {
        await EvLoadData();
        await ProductCategoryGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;
        await ProductCategoryState.LoadProductCategories();
        isLoading = false;
    }

    protected void EvEditRow(ProductCategoryDto productCategory)
    {
        NavigationManager.NavigateTo($"{ProductCategoryPageModel?.Path}/edit/{productCategory.CategoryID}");
    }

    protected async Task EvDeleteRow(ProductCategoryDto productCategory)
    {
        if (productCategory is null)
            return;

        string productCategoryName = productCategory.Name ?? string.Empty;
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Product Category", productCategoryName);
        if (!confirmationStatus)
            return;

        int productCategoryID = (int)productCategory.CategoryID!;
        var response = await ServiceManager.ProductCategoryService.Delete(productCategoryID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Product category has been deleted");
        await EvReloadData();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{ProductCategoryPageModel?.Path}/create");
    }

    private async Task PageChanged(PagerOnChangedEventArgs args)
    {
        ProductCategoryState.ProductCategoryParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }
}
