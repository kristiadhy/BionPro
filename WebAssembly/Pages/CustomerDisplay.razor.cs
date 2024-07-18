using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.CustomEventArgs;

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
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    CustomerState CustomerState { get; set; } = default!;

    internal static RadzenDataGrid<CustomerDTO> CustomerGrid { get; set; } = default!;

    private bool isLoading = false;
    private PageModel? CustomerPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }

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
        await CustomerState.LoadCustomers();
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
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Customer", customerName);
        if (!confirmationStatus)
            return;

        Guid customerID = (Guid)customer.CustomerID!;
        var response = await ServiceManager.CustomerService.Delete(customerID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Customer has been deleted");
        await EvReloadData();
    }

    protected void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{CustomerPageModel?.Path}/create");
    }

    private async Task PageChanged(PagerOnChangedEventArgs args)
    {
        CustomerState.CustomerParameter.PageNumber = args.CurrentPage;
        if (!args.IsFromFirstRender)
            await EvReloadData();
    }
}
