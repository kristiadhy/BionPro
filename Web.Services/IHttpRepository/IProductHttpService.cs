using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IProductHttpService
{
    public Task<PagingResponse<ProductDtoForProductQueries>> GetProducts(ProductParam productParam);
    public Task<ProductDto> GetProductByID(Guid productID);
    public Task<HttpResponseMessage> Create(ProductDto productDto);
    public Task<HttpResponseMessage> Update(ProductDto productDto);
    public Task<HttpResponseMessage> Delete(Guid productID);
    public Task<string> UploadProductImage(MultipartFormDataContent content);
    public Task<byte[]?> GetProductImage(string fileName);
    public Task<HttpResponseMessage> DeleteProductImage(string imageUrl);
}