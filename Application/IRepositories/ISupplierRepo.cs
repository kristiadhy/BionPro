using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;
public interface ISupplierRepo : IRepositoryBase<SupplierModel>
{
    Task<PagedList<SupplierModel>> GetAllAsync(SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PagedList<SupplierModel>> GetByParametersAsync(SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<SupplierModel?> GetByIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);
}
