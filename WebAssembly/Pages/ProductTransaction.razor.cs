using Blazored.FluentValidation;
using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;

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
    protected FluentValidationValidator ProductValidator;
    protected readonly string AdditionalHeaderText = "product";
    protected GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    protected bool IsSaving = false;
    protected ProductParam? ProductParameter = new();
    protected RadzenTextBox? txtNameForFocus;
    protected ProductImageUpload? ProductImageUploadRef;
    protected PageModel? ProductPageModel { get; set; }

    public ProductTransaction()
    {
        ProductPageModel = GlobalConstant.PageModels.Where(s => s.ID == 3).FirstOrDefault();
        ProductValidator = new();
    }

    protected override async Task OnParametersSetAsync()
    {
        if (ParamProductID is not null)
        {
            ProductState.Product = await ServiceManager.ProductService.GetProductByID((Guid)ParamProductID);
            FormStatus = GlobalEnum.FormStatus.Edit;
        }
        else
        {
            FormStatus = GlobalEnum.FormStatus.New;
            ProductState.Product.ProductID = null;
        }
    }

    public void EvBackToPrevious()
    {
        NavigationManager.NavigateTo($"{ProductPageModel?.Path}");
    }

    public async Task SubmitAsync(ProductDto product)
    {
        if (!await ConfirmationModalService.SavingConfirmation("Product"))
            return;

        IsSaving = true;

        try
        {
            // Handle image upload or deletion only if there's a change
            if (ProductImageUploadRef!.IsImageChanged)
            {
                if (FormStatus == GlobalEnum.FormStatus.Edit && product.ImageUrl is not null)
                    await ServiceManager.ProductService.DeleteProductImage(product.ImageUrl);

                product.ImageUrl = await ProductImageUploadRef!.StartUpload();
            }

            if (FormStatus == GlobalEnum.FormStatus.New)
                await ServiceManager.ProductService.Create(product);
            else
                await ServiceManager.ProductService.Update(product);

            var notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new product added" : "Product updated";
            NotificationService.SaveNotification(notificationMessage);

            await ProductState.LoadProducts();
            await ProductState.LoadProductsDropDown();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    //protected void OnUploadFileChanged(string fileName)
    //{
    //    ProductState.Product.ImageUrl = fileName;
    //}

    protected async Task ClearField()
    {
        ProductState.Product = new();
        await txtNameForFocus!.FocusAsync();
    }
}
