using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IProductHttpService
{
    public Task<PagingResponse<ProductDtoForProductQueries>> GetProducts(ProductParam productParam);
    public Task<ProductDto> GetProductByID(Guid productID);
    public Task Create(ProductDto productDto);
    public Task Update(ProductDto productDto);
    public Task Delete(Guid productID);
    public Task<string> UploadProductImage(MultipartFormDataContent content);
    public Task<byte[]?> GetProductImage(string fileName);
    public Task DeleteProductImage(string imageUrl);
}