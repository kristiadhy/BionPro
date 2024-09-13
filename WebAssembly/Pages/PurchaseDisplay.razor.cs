using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.CustomEventArgs;

namespace WebAssembly.Pages;

public partial class PurchaseDisplay
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
    DialogService DialogService { get; set; } = default!;
    [Inject]
    PurchaseState PurchaseState { get; set; } = default!;

    internal RadzenDataGrid<PurchaseDtoForSummary> PurchaseGrid = default!;

    protected bool isLoading = false;
    protected string filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
    protected string filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();

    protected PageModel? PurchasesPageModel { get; set; }
    protected IEnumerable<PageModel> BreadCrumbs { get; set; }
    protected Pager? Pager;

    public PurchaseDisplay()
    {
        PurchasesPageModel = GlobalConstant.PageModels.Where(s => s.ID == 5).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = PurchasesPageModel?.Path, Title= PurchasesPageModel?.Title },
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
        await PurchaseGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;
        await PurchaseState.LoadPurchasesForSummary();
        isLoading = false;
    }

    protected void EvEditRow(PurchaseDtoForSummary purchases)
    {
        NavigationManager.NavigateTo($"{PurchasesPageModel?.Path}/edit/{purchases.PurchaseID}");
    }

    protected async Task EvDeleteRow(PurchaseDtoForSummary purchases)
    {
        if (purchases is null)
            return;

        string transactionCode = purchases.TransactionCode ?? string.Empty;
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Purchase", $"Code {transactionCode}");
        if (!confirmationStatus)
            return;

        int purchaseID = (int)purchases.PurchaseID!;
        await ServiceManager.PurchaseService.Delete(purchaseID);

        NotificationService.DeleteNotification("Purchase data has been deleted");
        await EvReloadData();
    }

    protected async Task EvSeeDetail(PurchaseDtoForSummary purchases)
    {
        var parameters = new Dictionary<string, object>
        {
            { "purchaseID", purchases.PurchaseID! }
        };

        await DialogService.OpenAsync<PurchaseDetailItems>($"{purchases.TransactionCode} | {purchases.Date.ToString("dd/MM/yyyy HH:mm")}",
            parameters,
            new DialogOptions() { Width = "700px", Resizable = true, Draggable = true }
            );
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{PurchasesPageModel?.Path}/create");
    }

    protected async Task PageChanged(PagerOnChangedEventArgs args)
    {
        PurchaseState.PurchaseParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }

    protected async Task OnFilterButtonClick(RadzenSplitButtonItem item)
    {
        bool isFilterActiveBefore = PurchaseState.IsFilterActive;

        if (item is null)
        {
            //If the filter is active, then clear the filter
            if (isFilterActiveBefore)
                PurchaseState.ToggleFilterState();

            else //If there is no active filter, then set filter by transaction date as default
            {
                PurchaseState.IsFilterByDateActive = true;
                PurchaseState.IsFilterActive = true;
            }
        }
        else
        {
            // Use a method to set the filter state based on item.Value
            SetFilterStateBasedOnItemValue(item.Value);
        }

        // Update filter button appearance only if the filter activation state has changed
        if (isFilterActiveBefore != PurchaseState.IsFilterActive)
        {
            SetFilterButtonText();
            await Pager?.NavigateToPage(1)!;
        }
    }

    protected void SetFilterStateBasedOnItemValue(string value)
    {
        switch (value)
        {
            case nameof(FilterCondition.ByDate):
                PurchaseState.IsFilterByDateActive = true;
                break;
            case nameof(FilterCondition.BySupplier):
                PurchaseState.IsFilterBySupplierActive = true;
                break;
        }
        PurchaseState.IsFilterActive = true;
    }



    protected void SetFilterButtonText()
    {
        if (PurchaseState.IsFilterActive)
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
        PurchaseState.SetGlobalFilterStateByFilters();
        SetFilterButtonText();
    }

    protected enum FilterCondition
    {
        ByDate,
        BySupplier,
    }
}