using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Web.Services.IHttpRepository;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;
using WebAssembly.Constants;

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

    private readonly string AdditionalHeaderText = "supplier";
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected SupplierParam SupplierParameter = new();

    private PageModel? SupplierPageModel { get; set; }

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
            HttpResponseMessage response;
            if (FormStatus == GlobalEnum.FormStatus.New)
            {
                supplier.SupplierID = null;
                response = await ServiceManager.SupplierService.Create(supplier);
            }
            else
                response = await ServiceManager.SupplierService.Update(supplier);

            if (response.IsSuccessStatusCode)
            {
                string notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new supplier added" : "Supplier updated";
                NotificationService.SaveNotification(notificationMessage);
            }
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
