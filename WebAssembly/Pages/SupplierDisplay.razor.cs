using Domain.DTO;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;
using WebAssembly.Constants;

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
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    SupplierState SupplierState { get; set; } = default!;

    internal static RadzenDataGrid<SupplierDto> SupplierGrid { get; set; } = default!;

    private bool isLoading = false;
    private PageModel? SupplierPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }

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

    private async Task PageChanged(PagerEventArgs args)
    {
        SupplierState.SupplierParameter.PageNumber = args.PageIndex + 1;
        await EvReloadData();
    }
}
