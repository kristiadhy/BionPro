using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;

namespace Services.Contracts.IServices;
public interface IProductService : IServiceBase<ProductDto>
{
    Task<(IEnumerable<ProductDtoForProductQueries> productDto, MetaData metaData)> GetByParametersAsync(Guid productID, ProductParam productParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<ProductDto> GetByProductIDAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default);
    Task DeleteAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default);
    Task<(ProductDto productToPatch, ProductModel product)> GetProductForPatchAsync(Guid productID, bool empTrackChanges, CancellationToken cancellationToken = default);
    Task SaveChangesForPatchAsync(ProductDto productDto, ProductModel productModel, CancellationToken cancellationToken = default);
}
