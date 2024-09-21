using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Extensions;

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
    [CascadingParameter]
    ApplicationDetail? ApplicationDetail { get; set; }

    [Parameter] public Guid? ParamCustomerID { get; set; }
    protected readonly string AdditionalHeaderText = "customer";
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected CustomerParam CustomerParameter = new();
    protected RadzenTextBox? txtNameForFocus;

    protected PageModel? CustomerPageModel { get; set; }

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
            CustomerState.Customer.CustomerID = null;
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
            if (FormStatus == GlobalEnum.FormStatus.New)
            {
                customer.CustomerID = null;
                await ServiceManager.CustomerService.Create(customer);
            }
            else
                await ServiceManager.CustomerService.Update(customer);

            string notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new customer added" : "Customer updated";
            NotificationService.SaveNotification(notificationMessage);

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
