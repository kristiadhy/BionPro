using Application.IRepositories;
using Domain;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;

public sealed class CustomerRepo : MethodBase<CustomerModel>, ICustomerRepo
{
  public CustomerRepo(AppDBContext dbContext) : base(dbContext) { }

  public async Task<PagedList<CustomerModel>> GetAllAsync(CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var customers = await FindAll(trackChanges)
        .Sort(customerParam.OrderBy) //It's a local method
        .Skip((customerParam.PageNumber - 1) * customerParam.PageSize)
        .Take(customerParam.PageSize)
        .ToListAsync(cancellationToken);

    var count = await FindAll(trackChanges).CountAsync(cancellationToken);

    return new PagedList<CustomerModel>(customers, count, customerParam.PageNumber, customerParam.PageSize);
  }

  public async Task<PagedList<CustomerModel>> GetByParametersAsync(CustomerParam customerParam, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var customers = await FindAll(trackChanges)
        .SearchByName(customerParam.SrcByName) //It's a local method
        .Sort(customerParam.OrderBy) //It's a local method
        .Skip((customerParam.PageNumber - 1) * customerParam.PageSize)
        .Take(customerParam.PageSize)
        .ToListAsync(cancellationToken);

    var count = await FindAll(trackChanges)
        .SearchByName(customerParam.SrcByName)
        .CountAsync(cancellationToken);

    return new PagedList<CustomerModel>(customers, count, customerParam.PageNumber, customerParam.PageSize);
  }

  public async Task<CustomerModel?> GetByIDAsync(Guid customerID, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var customer = await FindByCondition(x => x.CustomerID == customerID, trackChanges).FirstOrDefaultAsync(cancellationToken);
    if (customer is not null)
      return customer;
    else
      return null;
  }

  public void CreateEntity(CustomerModel entity)
  {
    Create(entity);
  }

  public void UpdateEntity(CustomerModel entity)
  {
    Update(entity);
    IgnorePropertiesForUpdate(entity, nameof(BaseEntity.DateCreated), nameof(BaseEntity.CreatedBy));
  }

  public void DeleteEntity(CustomerModel entity)
  {
    Delete(entity);
  }
}
