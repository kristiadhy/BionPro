using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.CustomEventArgs;
using WebAssembly.Extensions;

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
    ProductCategoryDisplayState ProductCategoryDisplayState { get; set; } = default!;

    [CascadingParameter]
    ApplicationDetail? ApplicationDetail { get; set; }

    internal static RadzenDataGrid<ProductCategoryDto> ProductCategoryGrid { get; set; } = default!;

    protected bool isLoading = false;
    protected PageModel? ProductCategoryPageModel { get; set; }
    protected IEnumerable<PageModel> BreadCrumbs { get; set; }

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
        await ProductCategoryDisplayState.LoadProductCategories();
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
        if (!await ConfirmationModalService.DeleteConfirmation("Product Category", productCategoryName))
            return;

        int productCategoryID = (int)productCategory.CategoryID!;
        await ServiceManager.ProductCategoryService.Delete(productCategoryID);
        NotificationService.DeleteNotification("Product category has been deleted");

        await EvReloadData();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{ProductCategoryPageModel?.Path}/create");
    }

    protected async Task PageChanged(PagerOnChangedEventArgs args)
    {
        ProductCategoryDisplayState.ProductCategoryParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }
}
