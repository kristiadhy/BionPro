using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface IProductRepo : IRepositoryBase<ProductModel>
{
    Task<PagedList<ProductModel>> GetAllAsync(ProductParam productParam, bool trackChanges);
    Task<PagedList<ProductModel>> GetByParametersAsync(ProductParam productParam, bool trackChanges);
    Task<ProductModel?> GetByIDAsync(Guid productID, bool trackChanges);
}
