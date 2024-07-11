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

public partial class PurchaseDisplay
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
    PurchaseState PurchaseState { get; set; } = default!;

    private RadzenDataGrid<PurchaseDtoForQueries> PurchaseGrid { get; set; } = default!;

    private bool isLoading = false;

    private PageModel? PurchasesPageModel { get; set; }
    private IEnumerable<PageModel> BreadCrumbs { get; set; }

    public PurchaseDisplay()
    {
        PurchasesPageModel = GlobalConstant.PageModels.Where(s => s.ID == 5).FirstOrDefault();
        BreadCrumbs =
        [
            new PageModel { Path = PurchasesPageModel?.Path, Title= PurchasesPageModel?.Title },
            new PageModel { Path = null, Title = "List" }
        ];
    }

    private async Task EvReloadData()
    {
        await EvLoadData();
        await PurchaseGrid.Reload();
    }

    private async Task EvLoadData()
    {
        isLoading = true;
        await PurchaseState.LoadPurchases();
        isLoading = false;
    }

    private void EvEditRow(PurchaseDtoForQueries purchases)
    {
        NavigationManager.NavigateTo($"{PurchasesPageModel?.Path}/edit/{purchases.PurchaseID}");
    }

    private async Task EvDeleteRow(PurchaseDtoForQueries purchases)
    {
        if (purchases is null)
            return;

        string transactionCode = purchases.TransactionCode ?? string.Empty;
        bool confirmationStatus = await ConfirmationModalService.DeleteConfirmation("Purchase", $"Code {transactionCode}");
        if (!confirmationStatus)
            return;

        int purchaseID = purchases.PurchaseID!;
        var response = await ServiceManager.PurchaseService.Delete(purchaseID);
        if (!response.IsSuccessStatusCode)
            return;

        NotificationService.DeleteNotification("Purchase data has been deleted");
        await EvReloadData();
    }

    private void EvCreateNew()
    {
        NavigationManager.NavigateTo($"{PurchasesPageModel?.Path}/create");
    }

    private async Task PageChanged(PagerEventArgs args)
    {
        PurchaseState.PurchaseParameter.PageNumber = args.PageIndex + 1;
        await EvReloadData();
    }
}
