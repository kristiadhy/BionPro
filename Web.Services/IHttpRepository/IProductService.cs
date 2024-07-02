using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IProductService
{
    public Task<PagingResponse<ProductDto>> GetProductCategories(ProductParam productParam);
    public Task<ProductDto> GetProductByID(Guid productID);
    public Task<HttpResponseMessage> Create(ProductDto productDto);
    public Task<HttpResponseMessage> Update(ProductDto productDto);
    public Task<HttpResponseMessage> Delete(Guid productID);
}