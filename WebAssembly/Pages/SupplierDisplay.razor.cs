using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.CustomEventArgs;

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

    internal static RadzenDataGrid<SupplierDto> SupplierGrid { get; set; } = default!;

    private bool isLoading = false;
    private string filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
    private string filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();
    private PageModel? SupplierPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }
    private Pager? Pager;

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
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Supplier", supplierName);
        if (!confirmationStatus)
            return;

        Guid supplierID = (Guid)supplier.SupplierID!;
        var response = await ServiceManager.SupplierService.Delete(supplierID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Supplier has been deleted");
        await EvReloadData();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{SupplierPageModel?.Path}/create");
    }

    private async Task PageChanged(PagerOnChangedEventArgs args)
    {
        SupplierState.SupplierParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }

    private async Task OnFilterButtonClick(RadzenSplitButtonItem item)
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

    private void SetFilterStateBasedOnItemValue(string value)
    {
        switch (value)
        {
            case nameof(FilterCondition.BySupplierName):
                SupplierState.IsFilterBySupplierNameActive = true;
                break;
        }
        SupplierState.IsFilterActive = true;
    }

    private void SetFilterButtonText()
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

    private void ButtonClearFilterClicked()
    {
        SupplierState.SetGlobalFilterStateByFilters();
        SetFilterButtonText();
    }

    private enum FilterCondition
    {
        BySupplierName
    }
}
