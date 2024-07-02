using Blazored.FluentValidation;
using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;

namespace WebAssembly.Pages;

public partial class ProductCategoryTransaction
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
    ProductCategoryState ProductCategoryState { get; set; } = default!;

    [Parameter] public int? ParamProductCategoryID { get; set; }
    protected string PagePathText = string.Empty;
    protected string FormHeaderText = string.Empty;
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected ProductCategoryParam ProductCategoryParameter = new();
    private RadzenTextBox? txtNameForFocus;

    private PageModel? ProductCategoryPageModel { get; set; }

    public ProductCategoryTransaction()
    {
        ProductCategoryPageModel = GlobalState.PageModels.Where(s => s.ID == 4).FirstOrDefault();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamProductCategoryID is not null)
        {
            ProductCategoryState.ProductCategory = await ServiceManager.ProductCategoryService.GetProductCategoryByID((int)ParamProductCategoryID);

            PagePathText = GlobalEnum.FormStatus.Edit.ToString();
            FormHeaderText = $"{GlobalEnum.FormStatus.Edit.ToString()} Existing Product Category";
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            PagePathText = GlobalEnum.FormStatus.New.ToString();
            FormHeaderText = $"Create {GlobalEnum.FormStatus.New.ToString()} Product Category";
            FormStatus = GlobalEnum.FormStatus.New;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{ProductCategoryPageModel?.Path}");
    }

    public async Task SubmitAsync(ProductCategoryDto productCategory)
    {
        bool confirmationStatus = await ConfirmationModalService.SavingConfirmation("Product Category");
        if (!confirmationStatus)
            return;

        IsSaving = true;
        StateHasChanged();

        if (FormStatus == GlobalEnum.FormStatus.New)
        {
            var response = await ServiceManager.ProductCategoryService.Create(productCategory);
            if (response.IsSuccessStatusCode)
                NotificationService.SaveNotification("A new Product Category added");
        }
        else if (FormStatus == GlobalEnum.FormStatus.Edit)
        {
            var response = await ServiceManager.ProductCategoryService.Update(productCategory);
            if (response.IsSuccessStatusCode)
            {
                NotificationService.SaveNotification("Product Category updated");
            }
        }

        //Load productCategory state after making changes
        await ProductCategoryState.LoadProductCategories();

        IsSaving = false;
    }

    public async Task ClearField()
    {
        ProductCategoryState.ProductCategory = new();
        await txtNameForFocus!.FocusAsync();
    }
}
