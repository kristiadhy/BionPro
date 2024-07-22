using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;

public sealed class SupplierRepo : MethodBase<SupplierModel>, ISupplierRepo
{
    public SupplierRepo(AppDBContext dbContext) : base(dbContext) { }

    public async Task<PagedList<SupplierModel>> GetAllAsync(SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var suppliers = await FindAll(trackChanges)
            .Sort(supplierParam.OrderBy) //It's a local method
            .Skip((supplierParam.PageNumber - 1) * supplierParam.PageSize)
            .Take(supplierParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges).CountAsync(cancellationToken);

        return new PagedList<SupplierModel>(suppliers, count, supplierParam.PageNumber, supplierParam.PageSize);
    }

    public async Task<PagedList<SupplierModel>> GetByParametersAsync(SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var suppliers = await FindAll(trackChanges)
            .SearchByName(supplierParam.SrcByName) //It's a local method
            .Sort(supplierParam.OrderBy) //It's a local method
            .Skip((supplierParam.PageNumber - 1) * supplierParam.PageSize)
            .Take(supplierParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges)
            .SearchByName(supplierParam.SrcByName)
            .CountAsync(cancellationToken);

        return new PagedList<SupplierModel>(suppliers, count, supplierParam.PageNumber, supplierParam.PageSize);
    }

    public async Task<SupplierModel?> GetByIDAsync(Guid supplierID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var supplier = await FindByCondition(x => x.SupplierID == supplierID, trackChanges).FirstOrDefaultAsync(cancellationToken);
        if (supplier is not null)
            return supplier;
        else
            return null;
    }

    public void CreateEntity(SupplierModel entity)
    {
        Create(entity);
    }

    public void UpdateEntity(SupplierModel entity)
    {
        Update(entity);
    }

    public void DeleteEntity(SupplierModel entity)
    {
        Delete(entity);
    }
}
