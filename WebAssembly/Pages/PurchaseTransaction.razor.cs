using Blazored.FluentValidation;
using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
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
    PurchaseState PurchaseState { get; set; } = default!;
    ProductState ProductState { get; set; } = default!;
    PurchaseDetailState PurchaseDetailState { get; set; } = default!;

    [Parameter] public int? ParamPurchaseID { get; set; }
    private FluentValidationValidator PurchaseValidator;
    private string PagePathText = string.Empty;
    private string FormHeaderText = string.Empty;
    private GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    private bool IsSaving = false;
    private PurchaseParam? PurchaseParameter = new();
    private RadzenTextBox? txtForFocus;
    private int ProductSearchSelection = 1;
    private PageModel? PurchasePageModel { get; set; }
    private Variant FieldVariant = Variant.Outlined;
    private RadzenDataGrid<PurchaseDetailDto> PurchaseDetailGrid { get; set; } = default!;
    private bool GridIsLoading = false;

    public PurchaseTransaction()
    {
        PurchasePageModel = GlobalState.PageModels.Where(s => s.ID == 5).FirstOrDefault();
        PurchaseValidator = new();
    }

    protected override void OnInitialized()
    {
        //if (PurchaseState.Purchase.Date == default)
        //    PurchaseState.Purchase.Date = DateTimeOffset.Now;

        PurchaseState.PurchaseDetailForTransaction.ProductName = string.Empty;
        PurchaseState.PurchaseDetailForTransaction.Quantity = 1;
        PurchaseState.PurchaseDetailForTransaction.DiscountPercentage = 0;
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamPurchaseID is not null)
        {
            PurchaseState.Purchase = await ServiceManager.PurchaseService.GetPurchaseByID((int)ParamPurchaseID);

            PagePathText = GlobalEnum.FormStatus.Edit.ToString();
            FormHeaderText = $"{GlobalEnum.FormStatus.Edit.ToString()} Existing purchase transaction";
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            PagePathText = GlobalEnum.FormStatus.New.ToString();
            FormHeaderText = $"Create {GlobalEnum.FormStatus.New.ToString()} Purchase transaction";
            FormStatus = GlobalEnum.FormStatus.New;
        }

        await ProductState.LoadProductsDropDown();
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{PurchasePageModel?.Path}");
    }

    public async Task SubmitAsync(PurchaseDto product)
    {

    }

    private async Task ClearField()
    {
        PurchaseState.Purchase = new();
        await txtForFocus!.FocusAsync();
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

    private void OnProductChanged(Guid? value)
    {
        PurchaseState.PurchaseDetailForTransaction.ProductID = value;
    }

    private void EvEditQty(PurchaseDetailDto purchases)
    {

    }

    private void EvDeleteRow(PurchaseDetailDto purchases)
    {

    }
}
