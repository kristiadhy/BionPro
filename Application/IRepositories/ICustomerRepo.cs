using Domain.Entities;
using Domain.Parameters;

namespace Application.IRepositories;

public interface ICustomerRepo : IRepositoryBase<CustomerModel>
{
    Task<PagedList<CustomerModel>> GetAllAsync(CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<PagedList<CustomerModel>> GetByParametersAsync(CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default);
    Task<CustomerModel?> GetByIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default);
}
