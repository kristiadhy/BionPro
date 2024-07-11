using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
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
    protected string PagePathText = string.Empty;
    protected string FormHeaderText = string.Empty;
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected CustomerParam CustomerParameter = new();
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

            PagePathText = GlobalEnum.FormStatus.Edit.ToString();
            FormHeaderText = $"{GlobalEnum.FormStatus.Edit.ToString()} Existing Customer";
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            PagePathText = GlobalEnum.FormStatus.New.ToString();
            FormHeaderText = $"Create {GlobalEnum.FormStatus.New.ToString()} Customer";
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
