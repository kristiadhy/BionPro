using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.CustomEventArgs;
using WebAssembly.Extensions;

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
    CustomTooltipService CustomTooltipService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    ProductState ProductState { get; set; } = default!;
    [CascadingParameter]
    ApplicationDetail? ApplicationDetail { get; set; }

    internal static RadzenDataGrid<ProductDtoForProductQueries> ProductGrid { get; set; } = default!;

    protected bool isLoading = false;
    protected string filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
    protected string filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();

    protected PageModel? ProductsPageModel { get; set; }
    protected IEnumerable<PageModel> BreadCrumbs { get; set; }
    protected Pager? Pager;

    public ProductDisplay()
    {
        ProductsPageModel = GlobalConstant.PageModels.Where(s => s.ID == 3).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = ProductsPageModel?.Path, Title= ProductsPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    protected override void OnInitialized()
    {
        SetFilterButtonText();
    }

    protected async Task EvReloadData()
    {
        await EvLoadData();
        await ProductGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;
        await ProductState.LoadProducts();
        isLoading = false;
    }

    protected void EvEditRow(ProductDtoForProductQueries products)
    {
        NavigationManager.NavigateTo($"{ProductsPageModel?.Path}/edit/{products.ProductID}");
    }

    protected async Task EvDeleteRow(ProductDtoForProductQueries products)
    {
        if (products is null)
            return;

        string productName = products.Name ?? string.Empty;
        if (!await ConfirmationModalService.DeleteConfirmation("Product", productName))
            return;

        Guid productID = (Guid)products.ProductID!;
        if (products.ImageUrl is not null)
            await ServiceManager.ProductService.DeleteProductImage(products.ImageUrl);

        await ServiceManager.ProductService.Delete(productID);
        NotificationService.DeleteNotification("Product has been deleted");

        await EvReloadData();
        await ProductState.LoadProductsDropDown();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{ProductsPageModel?.Path}/create");
    }

    protected async Task PageChanged(PagerOnChangedEventArgs args)
    {
        ProductState.ProductParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }

    protected async Task OnFilterButtonClick(RadzenSplitButtonItem item)
    {
        bool isFilterActiveBefore = ProductState.IsFilterActive;

        //Click default filter button
        if (item is null)
        {
            //If the filter is active, then clear the filter
            if (isFilterActiveBefore)
                ProductState.ToggleFilterState();

            else //If there is no active filter, then set filter by product category as default
            {
                ProductState.IsFilterByProductCategoryActive = true;
                ProductState.IsFilterActive = true;
            }
        }
        else
        {
            // Use a method to set the filter state based on item.Value
            SetFilterStateBasedOnItemValue(item.Value);
        }

        // Update filter button appearance only if the filter activation state has changed
        if (isFilterActiveBefore != ProductState.IsFilterActive)
        {
            SetFilterButtonText();
            await Pager?.NavigateToPage(1)!;
        }
    }

    protected void SetFilterStateBasedOnItemValue(string value)
    {
        switch (value)
        {
            case nameof(FilterCondition.ByProductCategory):
                ProductState.IsFilterByProductCategoryActive = true;
                break;
            case nameof(FilterCondition.ByProductName):
                ProductState.IsFilterByProductNameActive = true;
                break;
        }
        ProductState.IsFilterActive = true;
    }

    protected void SetFilterButtonText()
    {
        if (ProductState.IsFilterActive)
        {
            filterText = GlobalEnum.FilterText.ClearFilters.GetDisplayDescription();
            filterIcon = GlobalEnum.FilterIcon.Cancel.GetDisplayDescription();
        }
        else
        {
            filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
            filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();
        }
    }

    protected void ButtonClearFilterClicked()
    {
        ProductState.SetGlobalFilterStateByFilters();
        SetFilterButtonText();
    }

    protected enum FilterCondition
    {
        ByProductCategory,
        ByProductName,
    }
}
