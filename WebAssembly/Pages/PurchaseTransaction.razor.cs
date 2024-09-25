using Blazored.FluentValidation;
using Domain.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.Extensions;

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
    PurchaseInputState PurchaseInputState { get; set; } = default!;
    [Inject]
    PurchaseDisplayState PurchaseDisplayState { get; set; } = default!;
    [Inject]
    ProductDropdownState ProductDropdownState { get; set; } = default!;

    [CascadingParameter]
    ApplicationDetail? ApplicationDetail { get; set; }

    [Parameter] public int? ParamPurchaseID { get; set; }

    internal FluentValidationValidator? PurchaseDetailValidator { get; set; }
    protected readonly string AdditionalHeaderText = "purchase transaction";

    protected GlobalEnum.FormStatus FormStatus;
    protected bool IsSaving = false;

    protected int ProductSearchSelection = 2;
    protected PageModel? PurchasePageModel { get; set; }
    protected readonly Variant FieldVariant = Variant.Outlined;
    protected RadzenDataGrid<PurchaseDetailDto> PurchaseDetailGrid = default!;
    protected bool GridIsLoading = false;
    protected PurchaseDetailDto PurchaseDetail = new();

    public PurchaseTransaction()
    {
        PurchasePageModel = GlobalConstant.PageModels.Where(s => s.ID == 5).FirstOrDefault();
    }

    protected override void OnInitialized()
    {
        SetPurchaseDetailDefaultValue(PurchaseDetail);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamPurchaseID is not null)
        {
            PurchaseInputState.PurchaseForTransaction = await ServiceManager.PurchaseService.GetPurchaseByID((int)ParamPurchaseID);
            FormStatus = GlobalEnum.FormStatus.Edit;
            await PurchaseDetailGrid.Reload();
        }
        else
        {
            FormStatus = GlobalEnum.FormStatus.New;
            PurchaseInputState.PurchaseForTransaction.PurchaseID = null;
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
            if (FormStatus == GlobalEnum.FormStatus.New)
                await ServiceManager.PurchaseService.Create(purchase);
            else
            {
                await ServiceManager.PurchaseService.Update(purchase);
                //IMPORTANT : After updating the purchase, PurchaseID need to be assigned to all of the purchase details. It's important because EF Core determine the the entity state of each details based on the ID. If there is no ID on the details, it will be identified as ADD state, otherwise it will be identified as UPDATE state. Since the new details are already added after update is executed, then the data model should be updated too.
                PurchaseInputState.PurchaseForTransaction.PurchaseDetails.ForEach(s => s.PurchaseID = PurchaseInputState.PurchaseForTransaction.PurchaseID);
            }

            var notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new purchase added" : "Purchase updated";
            NotificationService.SaveNotification(notificationMessage);

            await PurchaseDisplayState.LoadPurchasesForSummary();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    protected async Task ClearField()
    {
        PurchaseInputState.PurchaseForTransaction = new();
    }

    protected async Task AddToPurchaseDetailGrid(PurchaseDetailDto purchaseDetail)
    {
        GridIsLoading = true;
        var product = ProductDropdownState.ProductListDropdown.Where(s => s.ProductID == purchaseDetail.ProductID).FirstOrDefault();
        if (product != null)
        {
            purchaseDetail.ProductID = product.ProductID;
            purchaseDetail.ProductName = product.Name;
            purchaseDetail.Price = product.Price;

            PurchaseInputState.PurchaseForTransaction.PurchaseDetails.Add(purchaseDetail);
        }
        GridIsLoading = false;

        //This is used to re-validate the DataGrid, ensuring that error messages for non-existent products are not displayed.
        await PurchaseDetailValidator!.ValidateAsync();

        await PurchaseDetailGrid.Reload();

        PurchaseDetail = new();
        SetPurchaseDetailDefaultValue(PurchaseDetail);
    }

    protected void OnDateChanged(DateTime? dateTime)
    {
        PurchaseInputState.PurchaseForTransaction.Date = dateTime.HasValue ? new DateTimeOffset(dateTime.Value) : default;
    }

    protected void RefreshDate()
    {
        PurchaseInputState.PurchaseForTransaction.Date = DateTime.Now;
    }

    protected async Task EvEditDetails(PurchaseDetailDto purchaseDetail)
    {
        var parameters = new Dictionary<string, object>
        {
            { "PurchaseDetail", purchaseDetail }
        };

        await DialogService.OpenAsync<CustomPurchaseEditProductPopUp>($"{purchaseDetail.ProductName}", parameters);
    }

    protected async Task EvDeleteRow(PurchaseDetailDto purchaseDetail)
    {
        if (purchaseDetail is null)
            return;

        PurchaseInputState.PurchaseForTransaction.PurchaseDetails.Remove(purchaseDetail);
        await PurchaseDetailGrid.Reload();
    }

    protected void SetPurchaseDetailDefaultValue(PurchaseDetailDto purchaseDetailDto)
    {
        purchaseDetailDto.ProductID = null;
        purchaseDetailDto.ProductName = string.Empty;
        purchaseDetailDto.Quantity = 1;
        purchaseDetailDto.DiscountPercentage = 0;
    }
}
