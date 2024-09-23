using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.CustomEventArgs;
using WebAssembly.Extensions;

namespace WebAssembly.Pages;

public partial class SupplierDisplay
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
    SupplierState SupplierState { get; set; } = default!;

    [CascadingParameter]
    ApplicationDetail? ApplicationDetail { get; set; }

    internal static RadzenDataGrid<SupplierDto> SupplierGrid { get; set; } = default!;

    protected bool isLoading = false;
    protected string filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
    protected string filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();
    protected PageModel? SupplierPageModel { get; set; }
    protected IEnumerable<PageModel> BreadCrumbs { get; set; }
    protected Pager? Pager;

    public SupplierDisplay()
    {
        SupplierPageModel = GlobalConstant.PageModels.Where(s => s.ID == 2).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = SupplierPageModel?.Path, Title= SupplierPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    protected async Task EvReloadData()
    {
        await EvLoadData();
        await SupplierGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;
        await SupplierState.LoadSuppliers();
        isLoading = false;
    }

    protected void EvEditRow(SupplierDto supplier)
    {
        NavigationManager.NavigateTo($"{SupplierPageModel?.Path}/edit/{supplier.SupplierID}");
    }

    protected async Task EvDeleteRow(SupplierDto supplier)
    {
        if (supplier is null)
            return;

        string supplierName = supplier.SupplierName ?? string.Empty;
        if (!await ConfirmationModalService.DeleteConfirmation("Supplier", supplierName))
            return;

        Guid supplierID = (Guid)supplier.SupplierID!;
        await ServiceManager.SupplierService.Delete(supplierID);
        NotificationService.DeleteNotification("Supplier has been deleted");
        await EvReloadData();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{SupplierPageModel?.Path}/create");
    }

    protected async Task PageChanged(PagerOnChangedEventArgs args)
    {
        SupplierState.SupplierParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }

    protected async Task OnFilterButtonClick(RadzenSplitButtonItem item)
    {
        bool isFilterActiveBefore = SupplierState.IsFilterActive;

        //Click default filter button
        if (item is null)
        {
            //If the filter is active, then clear the filter
            if (isFilterActiveBefore)
                SupplierState.ToggleFilterState();

            else //If there is no active filter, then set filter by customer name as default
            {
                SupplierState.IsFilterBySupplierNameActive = true;
                SupplierState.IsFilterActive = true;
            }
        }
        else
        {
            // Use a method to set the filter state based on item.Value
            SetFilterStateBasedOnItemValue(item.Value);
        }

        // Update filter button appearance only if the filter activation state has changed
        if (isFilterActiveBefore != SupplierState.IsFilterActive)
        {
            SetFilterButtonText();
            await Pager?.NavigateToPage(1)!;
        }
    }

    protected void SetFilterStateBasedOnItemValue(string value)
    {
        switch (value)
        {
            case nameof(FilterCondition.BySupplierName):
                SupplierState.IsFilterBySupplierNameActive = true;
                break;
        }
        SupplierState.IsFilterActive = true;
    }

    protected void SetFilterButtonText()
    {
        if (SupplierState.IsFilterActive)
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
        SupplierState.SetGlobalFilterStateByFilters();
        SetFilterButtonText();
    }

    protected enum FilterCondition
    {
        BySupplierName
    }
}
