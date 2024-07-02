using Blazored.FluentValidation;
using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class ProductTransaction
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
    ProductState ProductState { get; set; } = default!;

    [Parameter] public Guid? ParamProductID { get; set; }
    private FluentValidationValidator ProductValidator;
    private string PagePathText = string.Empty;
    private string FormHeaderText = string.Empty;
    private GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    private bool IsSaving = false;
    private ProductParam? ProductParameter = new();
    private RadzenTextBox? txtNameForFocus;
    private RadzenUpload? uploadDD;
    private RadzenDropDown<int>? radzenDropDown;
    private int? SelectedCategoryID = null;

    private PageModel? ProductPageModel { get; set; }

    public ProductTransaction()
    {
        ProductPageModel = GlobalState.PageModels.Where(s => s.ID == 5).FirstOrDefault();
        ProductValidator = new();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamProductID is not null)
        {
            ProductState.Product = await ServiceManager.ProductService.GetProductByID((Guid)ParamProductID);

            PagePathText = GlobalEnum.FormStatus.Edit.ToString();
            FormHeaderText = $"{GlobalEnum.FormStatus.Edit.ToString()} Existing Product ";
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            PagePathText = GlobalEnum.FormStatus.New.ToString();
            FormHeaderText = $"Create {GlobalEnum.FormStatus.New.ToString()} Product ";
            FormStatus = GlobalEnum.FormStatus.New;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{ProductPageModel?.Path}");
    }

    public async Task SubmitAsync(ProductDto product)
    {
        bool confirmationStatus = await ConfirmationModalService.SavingConfirmation("Product");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        StateHasChanged();

        if (FormStatus == GlobalEnum.FormStatus.New)
        {
            var response = await ServiceManager.ProductService.Create(product);
            if (response.IsSuccessStatusCode)
                NotificationService.SaveNotification("A new Product added");
        }
        else if (FormStatus == GlobalEnum.FormStatus.Edit)
        {
            var response = await ServiceManager.ProductService.Update(product);
            if (response.IsSuccessStatusCode)
            {
                NotificationService.SaveNotification("Product updated");
            }
        }

        await ProductState.LoadProductCategories();

        IsSaving = false;
    }

    public async Task ClearField()
    {
        ProductState.Product = new();
        await txtNameForFocus!.FocusAsync();
    }

    private void OnProductCategoryChanged(int? value)
    {
        ProductState.Product.CategoryID = value;
        ProductValidator.Validate();
    }

    private void OnProgressUpload(UploadProgressArgs args, string name)
    {
        if (args.Progress == 100)
        {
            foreach (var file in args.Files)
            {

            }
        }
    }
}
