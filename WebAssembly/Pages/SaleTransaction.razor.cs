using Blazored.FluentValidation;
using Domain.DTO;
using FluentValidation;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;

namespace WebAssembly.Pages;

public partial class SaleTransaction
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
    SaleState SaleState { get; set; } = default!;
    [Inject]
    ProductState ProductState { get; set; } = default!;

    [Parameter] public int? ParamSaleID { get; set; }

    private FluentValidationValidator? SaleDetailValidator { get; set; }

    private readonly string AdditionalHeaderText = "sale transaction";
    private GlobalEnum.FormStatus FormStatus;
    private bool IsSaving = false;

    private int ProductSearchSelection = 2;
    private PageModel? SalePageModel { get; set; }
    private readonly Variant FieldVariant = Variant.Outlined;
    private RadzenDataGrid<SaleDetailDto> SaleDetailGrid = default!;
    private bool GridIsLoading = false;
    private SaleDetailDto SaleDetail = new();

    public SaleTransaction()
    {
        SalePageModel = GlobalConstant.PageModels.Where(s => s.ID == 6).FirstOrDefault();
    }

    protected override void OnInitialized()
    {
        SetSaleDetailDefaultValue(SaleDetail);
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamSaleID is not null)
        {
            SaleState.SaleForTransaction = await ServiceManager.SaleService.GetSaleByID((int)ParamSaleID);
            FormStatus = GlobalEnum.FormStatus.Edit;
            await SaleDetailGrid.Reload();
        }
        else
        {
            FormStatus = GlobalEnum.FormStatus.New;
            SaleState.SaleForTransaction.SaleID = null;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{SalePageModel?.Path}");
    }

    public async Task SubmitAsync(SaleDto sale)
    {
        if (!await ConfirmationModalService.SavingConfirmation("Sale"))
            return;

        IsSaving = true;

        try
        {
            HttpResponseMessage response;
            if (FormStatus == GlobalEnum.FormStatus.New)
                response = await ServiceManager.SaleService.Create(sale);
            else
            {
                response = await ServiceManager.SaleService.Update(sale);
                //IMPORTANT : After updating the sale, SaleID need to be assigned to all of the sale details. It's important because EF Core determine the the entity state of each details based on the ID. If there is no ID on the details, it will be identified as ADD state, otherwise it will be identified as UPDATE state. Since the new details are already added after update is executed, then the data model should be updated too.
                SaleState.SaleForTransaction.SaleDetails.ForEach(s => s.SaleID = SaleState.SaleForTransaction.SaleID);
            }

            if (response.IsSuccessStatusCode)
            {
                var notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new sale added" : "Sale updated";
                NotificationService.SaveNotification(notificationMessage);
            }

            await SaleState.LoadSalesForSummary();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    private async Task ClearField()
    {
        SaleState.SaleForTransaction = new();
    }

    private async Task AddToSaleDetailGrid(SaleDetailDto saleDetail)
    {
        GridIsLoading = true;
        var product = ProductState.ProductListDropdown.Where(s => s.ProductID == saleDetail.ProductID).FirstOrDefault();
        if (product != null)
        {
            saleDetail.ProductID = product.ProductID;
            saleDetail.ProductName = product.Name;
            saleDetail.Price = product.Price;
            SaleState.SaleForTransaction.SaleDetails.Add(saleDetail);
        }
        GridIsLoading = false;

        //This is used to re-validate the DataGrid, ensuring that error messages for non-existent products are not displayed.
        await SaleDetailValidator!.ValidateAsync();

        await SaleDetailGrid.Reload();

        SaleDetail = new();
        SetSaleDetailDefaultValue(SaleDetail);
    }

    private void OnDateChanged(DateTime? dateTime)
    {
        SaleState.SaleForTransaction.Date = dateTime.HasValue ? new DateTimeOffset(dateTime.Value) : default;
    }

    private void RefreshDate()
    {
        SaleState.SaleForTransaction.Date = DateTime.Now;
    }

    private async Task EvEditDetails(SaleDetailDto saleDetail)
    {
        var parameters = new Dictionary<string, object>
        {
            { "SaleDetail", saleDetail }
        };

        await DialogService.OpenAsync<CustomSaleEditProductPopUp>($"{saleDetail.ProductName}", parameters);
    }

    private async Task EvDeleteRow(SaleDetailDto saleDetail)
    {
        if (saleDetail is null)
            return;

        SaleState.SaleForTransaction.SaleDetails.Remove(saleDetail);
        await SaleDetailGrid.Reload();
    }

    private void SetSaleDetailDefaultValue(SaleDetailDto saleDetailDto)
    {
        saleDetailDto.ProductID = null;
        saleDetailDto.ProductName = string.Empty;
        saleDetailDto.Quantity = 1;
        saleDetailDto.DiscountPercentage = 0;
    }
}
