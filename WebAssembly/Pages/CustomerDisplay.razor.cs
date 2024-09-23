using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Microsoft.Extensions.Options;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.CustomEventArgs;
using WebAssembly.Extensions;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace WebAssembly.Pages;

public partial class CustomerDisplay
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
    CustomerState CustomerState { get; set; } = default!;

    [CascadingParameter]
    ApplicationDetail? ApplicationDetail { get; set; }

    internal static RadzenDataGrid<CustomerDTO> CustomerGrid { get; set; } = default!;

    protected List<CustomerDTO> customers = [];
    protected MetaData metaData = new();

    protected bool isLoading = false;
    protected string filterText = GlobalEnum.FilterText.AddFilter.GetDisplayDescription();
    protected string filterIcon = GlobalEnum.FilterIcon.Search.GetDisplayDescription();
    protected PageModel? CustomerPageModel { get; set; }
    protected IEnumerable<PageModel> BreadCrumbs { get; set; }
    protected Pager? Pager;

    public CustomerDisplay()
    {
        CustomerPageModel = GlobalConstant.PageModels.Where(s => s.ID == 1).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = CustomerPageModel?.Path, Title= CustomerPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    protected async Task EvReloadData()
    {
        await EvLoadData();
        await CustomerGrid.Reload();
    }

    protected async Task EvLoadData()
    {
        isLoading = true;
        CustomerState.UpdateCustomerParametersBasedOnActiveFilters();
        var paginResponse = await ServiceManager.CustomerService.GetCustomers(CustomerState.CustomerParameter);
        customers = paginResponse.Items;
        metaData = paginResponse.MetaData;
        isLoading = false;
    }

    protected void EvEditRow(CustomerDTO customer)
    {
        NavigationManager.NavigateTo($"{CustomerPageModel?.Path}/edit/{customer.CustomerID}");
    }

    protected async Task EvDeleteRow(CustomerDTO customer)
    {
        if (customer is null)
            return;

        string customerName = customer.CustomerName ?? string.Empty;
        if (!await ConfirmationModalService.DeleteConfirmation("Customer", customerName))
            return;

        Guid customerID = (Guid)customer.CustomerID!;
        await ServiceManager.CustomerService.Delete(customerID);

        NotificationService.DeleteNotification("Customer has been deleted");
        await EvReloadData();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{CustomerPageModel?.Path}/create");
    }

    protected async Task PageChanged(PagerOnChangedEventArgs args)
    {
        CustomerState.CustomerParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }

    protected async Task OnFilterButtonClick(RadzenSplitButtonItem item)
    {
        bool isFilterActiveBefore = CustomerState.IsFilterActive;

        //Click default filter button
        if (item is null)
        {
            //If the filter is active, then clear the filter
            if (isFilterActiveBefore)
                CustomerState.ToggleFilterState();

            else //If there is no active filter, then set filter by customer name as default
            {
                CustomerState.IsFilterByCustomerNameActive = true;
                CustomerState.IsFilterActive = true;
            }
        }
        else
        {
            // Use a method to set the filter state based on item.Value
            SetFilterStateBasedOnItemValue(item.Value);
        }

        // Update filter button appearance only if the filter activation state has changed
        if (isFilterActiveBefore != CustomerState.IsFilterActive)
        {
            SetFilterButtonText();
            await Pager?.NavigateToPage(1)!;
        }
    }

    protected void SetFilterStateBasedOnItemValue(string value)
    {
        switch (value)
        {
            case nameof(FilterCondition.ByCustomerName):
                CustomerState.IsFilterByCustomerNameActive = true;
                break;
        }
        CustomerState.IsFilterActive = true;
    }

    protected void SetFilterButtonText()
    {
        if (CustomerState.IsFilterActive)
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
        CustomerState.SetGlobalFilterStateByFilters();
        SetFilterButtonText();
    }

    protected enum FilterCondition
    {
        ByCustomerName
    }
}
