using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;

namespace WebAssembly.Pages;

public partial class SupplierTransaction
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
    SupplierState SupplierState { get; set; } = default!;

    [Parameter] public Guid? ParamSupplierID { get; set; }

    protected readonly string AdditionalHeaderText = "supplier";
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected SupplierParam SupplierParameter = new();

    protected PageModel? SupplierPageModel { get; set; }

    public SupplierTransaction()
    {
        SupplierPageModel = GlobalConstant.PageModels.Where(s => s.ID == 2).FirstOrDefault();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamSupplierID is not null)
        {
            SupplierState.Supplier = await ServiceManager.SupplierService.GetSupplierByID((Guid)ParamSupplierID);
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            FormStatus = GlobalEnum.FormStatus.New;
            SupplierState.Supplier.SupplierID = null;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{SupplierPageModel?.Path}");
    }

    public async Task SubmitAsync(SupplierDto supplier)
    {
        bool confirmationStatus = await ConfirmationModalService.SavingConfirmation("Supplier");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        StateHasChanged();

        try
        {
            if (FormStatus == GlobalEnum.FormStatus.New)
            {
                supplier.SupplierID = null;
                await ServiceManager.SupplierService.Create(supplier);
            }
            else
                await ServiceManager.SupplierService.Update(supplier);

            string notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new supplier added" : "Supplier updated";
            NotificationService.SaveNotification(notificationMessage);

            await SupplierState.LoadSuppliers();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    public void ClearField()
    {
        SupplierState.Supplier = new();
    }
}
