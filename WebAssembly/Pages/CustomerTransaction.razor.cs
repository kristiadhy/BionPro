using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.Constants;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class CustomerTransaction
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
    CustomerState CustomerState { get; set; } = default!;

    [Parameter] public Guid? ParamCustomerID { get; set; }
    private readonly string AdditionalHeaderText = "customer";
    private GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    private bool IsSaving = false;
    private CustomerParam CustomerParameter = new();
    private RadzenTextBox? txtNameForFocus;

    private PageModel? CustomerPageModel { get; set; }

    public CustomerTransaction()
    {
        CustomerPageModel = GlobalConstant.PageModels.Where(s => s.ID == 1).FirstOrDefault();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamCustomerID is not null)
        {
            CustomerState.Customer = await ServiceManager.CustomerService.GetCustomerByID((Guid)ParamCustomerID);
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            FormStatus = GlobalEnum.FormStatus.New;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{CustomerPageModel?.Path}");
    }

    public async Task SubmitAsync(CustomerDTO customer)
    {
        if (!await ConfirmationModalService.SavingConfirmation("Customer"))
            return;

        IsSaving = true;
        try
        {
            HttpResponseMessage response;
            if (FormStatus == GlobalEnum.FormStatus.New)
            {
                customer.CustomerID = null;
                response = await ServiceManager.CustomerService.Create(customer);
            }
            else
                response = await ServiceManager.CustomerService.Update(customer);

            if (response.IsSuccessStatusCode)
            {
                string notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new customer added" : "Customer updated";
                NotificationService.SaveNotification(notificationMessage);
            }

            await CustomerState.LoadCustomers();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    public async Task ClearField()
    {
        CustomerState.Customer = new();
        await txtNameForFocus!.FocusAsync();
    }
}
