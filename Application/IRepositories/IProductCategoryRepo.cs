using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface IProductCategoryRepo : IRepositoryBase<ProductCategoryModel>
{
    Task<PagedList<ProductCategoryModel>> GetAllAsync(ProductCategoryParam productCategoryParam, bool trackChanges);
    Task<PagedList<ProductCategoryModel>> GetByParametersAsync(ProductCategoryParam productCategoryParam, bool trackChanges);
    Task<ProductCategoryModel?> GetByIDAsync(int categoryID, bool trackChanges);
}
