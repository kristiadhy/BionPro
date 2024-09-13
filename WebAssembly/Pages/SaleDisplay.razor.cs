using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.CustomEventArgs;

namespace WebAssembly.Pages;

public partial class SaleDisplay
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
    SaleState SaleState { get; set; } = default!;

    internal RadzenDataGrid<SaleDtoForSummary>? SaleGrid { get; set; } = default!;

    protected bool isLoading = false;
    protected string filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
    protected string filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();

    protected PageModel? SalesPageModel { get; set; }
    protected IEnumerable<PageModel> BreadCrumbs { get; set; }
    protected Pager? Pager;

    public SaleDisplay()
    {
        SalesPageModel = GlobalConstant.PageModels.Where(s => s.ID == 6).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = SalesPageModel?.Path, Title= SalesPageModel?.Title },
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
        await SaleGrid?.Reload()!;
    }

    protected async Task EvLoadData()
    {
        isLoading = true;
        await SaleState.LoadSalesForSummary();
        isLoading = false;
    }

    protected void EvEditRow(SaleDtoForSummary sales)
    {
        NavigationManager.NavigateTo($"{SalesPageModel?.Path}/edit/{sales.SaleID}");
    }

    protected async Task EvDeleteRow(SaleDtoForSummary sales)
    {
        if (sales is null)
            return;

        string transactionCode = sales.TransactionCode ?? string.Empty;
        if (!await ConfirmationModalService.DeleteConfirmation("Sale", $"Code {transactionCode}"))
            return;

        int saleID = (int)sales.SaleID!;
        await ServiceManager.SaleService.Delete(saleID);

        NotificationService.DeleteNotification("Sale data has been deleted");
        await EvReloadData();
    }

    protected async Task EvSeeDetail(SaleDtoForSummary sales)
    {
        var parameters = new Dictionary<string, object>
        {
            { "saleID", sales.SaleID! }
        };

        await DialogService.OpenAsync<SaleDetailItems>($"{sales.TransactionCode} | {sales.Date.ToString("dd/MM/yyyy HH:mm")}",
            parameters,
            new DialogOptions() { Width = "700px", Resizable = true, Draggable = true }
            );
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{SalesPageModel?.Path}/create");
    }

    protected async Task PageChanged(PagerOnChangedEventArgs args)
    {
        SaleState.SaleParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }

    protected async Task OnFilterButtonClick(RadzenSplitButtonItem item)
    {
        bool isFilterActiveBefore = SaleState.IsFilterActive;

        if (item is null)
        {
            //If the filter is active, then clear the filter
            if (isFilterActiveBefore)
                SaleState.ToggleFilterState();

            else //If there is no active filter, then set filter by transaction date as default
            {
                SaleState.IsFilterByDateActive = true;
                SaleState.IsFilterActive = true;
            }
        }
        else
        {
            // Use a method to set the filter state based on item.Value
            SetFilterStateBasedOnItemValue(item.Value);
        }

        // Update filter button appearance only if the filter activation state has changed
        if (isFilterActiveBefore != SaleState.IsFilterActive)
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
                SaleState.IsFilterByDateActive = true;
                break;
            case nameof(FilterCondition.ByCustomer):
                SaleState.IsFilterByCustomerActive = true;
                break;
        }
        SaleState.IsFilterActive = true;
    }



    protected void SetFilterButtonText()
    {
        if (SaleState.IsFilterActive)
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
        SaleState.SetGlobalFilterStateByFilters();
        SetFilterButtonText();
    }

    protected enum FilterCondition
    {
        ByDate,
        ByCustomer,
    }
}