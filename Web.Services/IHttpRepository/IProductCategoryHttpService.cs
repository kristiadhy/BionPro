using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IProductCategoryHttpService
{
    public Task<PagingResponse<ProductCategoryDto>> GetProductCategories(ProductCategoryParam productCategoryParam);
    public Task<ProductCategoryDto> GetProductCategoryByID(int productCategoryID);
    public Task<HttpResponseMessage> Create(ProductCategoryDto productCategoryDto);
    public Task<HttpResponseMessage> Update(ProductCategoryDto productCategoryDto);
    public Task<HttpResponseMessage> Delete(int productCategoryID);
}
