using Blazored.FluentValidation;
using Domain.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.Constants;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class PurchaseTransaction
{
    [Inject]
    NavigationManager NavigationManager { get; set; } = default!;
    [Inject]
    CustomModalService ConfirmationModalService { get; set; } = default!;
    [Inject]
    CustomNotificationService NotificationService { get; set; } = default!;
    [Inject]
    IServiceManager ServiceManager { get; set; } = default!;
    [Inject]
    DialogService DialogService { get; set; } = default!;
    [Inject]
    PurchaseState PurchaseState { get; set; } = default!;
    [Inject]
    ProductState ProductState { get; set; } = default!;

    [Parameter] public int? ParamPurchaseID { get; set; }

    private FluentValidationValidator? PurchaseDetailValidator { get; set; }

    private string PagePathText = string.Empty;
    private string FormHeaderText = string.Empty;
    private GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    private bool IsSaving = false;

    private int ProductSearchSelection = 2;
    private PageModel? PurchasePageModel { get; set; }
    private readonly Variant FieldVariant = Variant.Outlined;
    private RadzenDataGrid<PurchaseDetailDto> PurchaseDetailGrid = default!;
    private bool GridIsLoading = false;
    private PurchaseDetailDto PurchaseDetail = new();

    public PurchaseTransaction()
    {
        PurchasePageModel = GlobalConstant.PageModels.Where(s => s.ID == 5).FirstOrDefault();
    }

    protected override void OnInitialized()
    {
        //if (PurchaseState.Purchase.Date == default)
        //    PurchaseState.Purchase.Date = DateTimeOffset.Now;

        SetPurchaseDetailDefaultValue(PurchaseDetail);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamPurchaseID is not null)
        {
            PurchaseState.Purchase = await ServiceManager.PurchaseService.GetPurchaseByID((int)ParamPurchaseID);

            PagePathText = GlobalEnum.FormStatus.Edit.ToString();
            FormHeaderText = $"{GlobalEnum.FormStatus.Edit.ToString()} Existing purchase transaction";
            FormStatus = GlobalEnum.FormStatus.Edit;
            await PurchaseDetailGrid.Reload();
        }
        else
        {
            PagePathText = GlobalEnum.FormStatus.New.ToString();
            FormHeaderText = $"Create {GlobalEnum.FormStatus.New.ToString()} Purchase transaction";
            FormStatus = GlobalEnum.FormStatus.New;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{PurchasePageModel?.Path}");
    }

    public async Task SubmitAsync(PurchaseDto purchase)
    {
        if (!await ConfirmationModalService.SavingConfirmation("Purchase"))
            return;

        IsSaving = true;

        try
        {
            HttpResponseMessage response;
            if (FormStatus == GlobalEnum.FormStatus.New)
                response = await ServiceManager.PurchaseService.Create(purchase);
            else
                response = await ServiceManager.PurchaseService.Update(purchase);

            if (response.IsSuccessStatusCode)
            {
                var notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new purchase added" : "Purchase updated";
                NotificationService.SaveNotification(notificationMessage);
            }

            await PurchaseState.LoadPurchases();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    private async Task ClearField()
    {
        PurchaseState.Purchase = new();
    }

    private async Task AddToPurchaseDetailGrid(PurchaseDetailDto purchaseDetail)
    {
        GridIsLoading = true;
        var product = ProductState.ProductListDropdown.Where(s => s.ProductID == purchaseDetail.ProductID).FirstOrDefault();
        if (product != null)
        {
            purchaseDetail.ProductID = product.ProductID;
            purchaseDetail.ProductName = product.Name;
            purchaseDetail.Price = product.Price;
            PurchaseState.Purchase.PurchaseDetails.Add(purchaseDetail);
        }
        GridIsLoading = false;

        //This is used to re-validate the DataGrid, ensuring that error messages for non-existent products are not displayed.
        await PurchaseDetailValidator!.ValidateAsync();

        await PurchaseDetailGrid.Reload();

        PurchaseDetail = new();
        SetPurchaseDetailDefaultValue(PurchaseDetail);
    }

    private void OnSupplierChanged(Guid? value)
    {
        PurchaseState.Purchase.SupplierID = value;
    }

    private void OnDateChanged(DateTime? dateTime)
    {
        PurchaseState.Purchase.Date = dateTime.HasValue ? new DateTimeOffset(dateTime.Value) : default;
    }

    private void RefreshDate()
    {
        PurchaseState.Purchase.Date = DateTime.Now;
    }

    private async Task EvEditDetails(PurchaseDetailDto purchaseDetail)
    {
        var parameters = new Dictionary<string, object>
        {
            { "PurchaseDetail", purchaseDetail }
        };

        await DialogService.OpenAsync<CustomProductSettingPopUp>($"{purchaseDetail.ProductName}", parameters);
    }

    private async Task EvDeleteRow(PurchaseDetailDto purchaseDetail)
    {
        if (purchaseDetail is null)
            return;

        PurchaseState.Purchase.PurchaseDetails.Remove(purchaseDetail);
        await PurchaseDetailGrid.Reload();
    }

    private void SetPurchaseDetailDefaultValue(PurchaseDetailDto purchaseDetailDto)
    {
        purchaseDetailDto.ProductID = null;
        purchaseDetailDto.ProductName = string.Empty;
        purchaseDetailDto.Quantity = 1;
        purchaseDetailDto.DiscountPercentage = 0;
    }
}
