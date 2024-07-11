using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;
using WebAssembly.Constants;

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
        ProductCategoryPageModel = GlobalConstant.PageModels.Where(s => s.ID == 4).FirstOrDefault();
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
        if (!confirmationStatus) return;

        IsSaving = true;
        try
        {

            var response = FormStatus == GlobalEnum.FormStatus.New
            ? await ServiceManager.ProductCategoryService.Create(productCategory)
            : await ServiceManager.ProductCategoryService.Update(productCategory);

            if (response.IsSuccessStatusCode)
            {
                string notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new product category added" : "Product category updated";
                NotificationService.SaveNotification(notificationMessage);
            }

            await ProductCategoryState.LoadProductCategories();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }


    public async Task ClearField()
    {
        ProductCategoryState.ProductCategory = new();
        await txtNameForFocus!.FocusAsync();
    }
}
