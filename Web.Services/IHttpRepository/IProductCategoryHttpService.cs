using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;
public interface IProductCategoryHttpService
{
  public Task<PagingResponse<ProductCategoryDto>> GetProductCategories(ProductCategoryParam productCategoryParam);
  public Task<ProductCategoryDto> GetProductCategoryByID(int productCategoryID);
  public Task Create(ProductCategoryDto productCategoryDto);
  public Task Update(ProductCategoryDto productCategoryDto);
  public Task Delete(int productCategoryID);
}
