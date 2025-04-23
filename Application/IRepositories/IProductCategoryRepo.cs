using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface IProductCategoryRepo : IRepositoryBase<ProductCategoryModel>
{
  Task<PagedList<ProductCategoryModel>> GetAllAsync(ProductCategoryParam productCategoryParam, bool trackChanges, CancellationToken cancellationToken = default);
  Task<PagedList<ProductCategoryModel>> GetByParametersAsync(ProductCategoryParam productCategoryParam, bool trackChanges, CancellationToken cancellationToken = default);
  Task<ProductCategoryModel?> GetByIDAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default);
}
