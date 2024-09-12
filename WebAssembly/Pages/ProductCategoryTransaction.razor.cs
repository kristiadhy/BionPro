using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen.Blazor;
using Web.Services.IHttpRepository;

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
    private readonly string AdditionalHeaderText = "product category";
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
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            FormStatus = GlobalEnum.FormStatus.New;
            ProductCategoryState.ProductCategory.CategoryID = null;
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
            if (FormStatus == GlobalEnum.FormStatus.New)
            {
                await ServiceManager.ProductCategoryService.Create(productCategory);
            }
            else
                await ServiceManager.ProductCategoryService.Update(productCategory);

            string notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new product category added" : "Product category updated";
            NotificationService.SaveNotification(notificationMessage);

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
