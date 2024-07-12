using Blazored.FluentValidation;
using Domain.DTO;
using Domain.Parameters;
using Microsoft.AspNetCore.Components;
using Radzen;
using Radzen.Blazor;
using Web.Services.IHttpRepository;
using WebAssembly.Components;
using WebAssembly.Model;
using WebAssembly.Services;
using WebAssembly.StateManagement;
using WebAssembly.Constants;

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
    private readonly string AdditionalHeaderText = "product";
    private GlobalEnum.FormStatus FormStatus = GlobalEnum.FormStatus.New;
    private bool IsSaving = false;
    private ProductParam? ProductParameter = new();
    private RadzenTextBox? txtNameForFocus;
    private ProductImageUpload? ProductImageUploadRef;
    private PageModel? ProductPageModel { get; set; }

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

            HttpResponseMessage response;
            if (FormStatus == GlobalEnum.FormStatus.New)
                response = await ServiceManager.ProductService.Create(product);
            else
                response = await ServiceManager.ProductService.Update(product);

            if (response.IsSuccessStatusCode)
            {
                var notificationMessage = FormStatus == GlobalEnum.FormStatus.New ? "A new product added" : "Product updated";
                NotificationService.SaveNotification(notificationMessage);
            }

            await ProductState.LoadProducts();
        }
        finally
        {
            IsSaving = false;
            StateHasChanged();
        }
    }

    //private void OnUploadFileChanged(string fileName)
    //{
    //    ProductState.Product.ImageUrl = fileName;
    //}

    private async Task ClearField()
    {
        ProductState.Product = new();
        await txtNameForFocus!.FocusAsync();
    }
}
