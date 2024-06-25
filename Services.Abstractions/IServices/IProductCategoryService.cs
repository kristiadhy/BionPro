using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts.IServices;
public interface IProductCategoryService : IServiceBase<ProductCategoryDto>
{
    Task<(IEnumerable<ProductCategoryDto> productCategoryDto, MetaData metaData)> GetByParametersAsync(int categoryID, ProductCategoryParam productCategoryParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ProductCategoryDto> GetByProductCategoryIDAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default);
    Task<(ProductCategoryDto productCategoryToPatch, ProductCategoryModel productCategory)> GetProductCategoryForPatchAsync(int categoryID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(ProductCategoryDto productCategoryDto, ProductCategoryModel productCategoryModel, CancellationToken cancellationToken = default);
}
