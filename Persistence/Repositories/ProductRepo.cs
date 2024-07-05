using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;

public sealed class ProductRepo : MethodBase<ProductModel>, IProductRepo
{
    public ProductRepo(AppDBContext dbContext) : base(dbContext) { }

    public async Task<PagedList<ProductModel>> GetAllAsync(ProductParam productParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategories = await FindAll(trackChanges)
            .Include(x => x.Category)
            .Sort(productParam.OrderBy)
            .Skip((productParam.PageNumber - 1) * productParam.PageSize)
            .Take(productParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges).CountAsync(cancellationToken);

        return new PagedList<ProductModel>(productCategories, count, productParam.PageNumber, productParam.PageSize);
    }

    public async Task<PagedList<ProductModel>> GetByParametersAsync(ProductParam productParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategories = await FindAll(trackChanges)
            .Include(x => x.Category)
            .SearchByName(productParam.srcByName) //It's a local method
            .Sort(productParam.OrderBy)
            .Skip((productParam.PageNumber - 1) * productParam.PageSize)
            .Take(productParam.PageSize)
            .ToListAsync(cancellationToken);

        var count = await FindAll(trackChanges)
            .SearchByName(productParam.srcByName)
            .CountAsync(cancellationToken);

        return new PagedList<ProductModel>(productCategories, count, productParam.PageNumber, productParam.PageSize);
    }

    public async Task<ProductModel?> GetByIDAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var product = await FindByCondition(x => x.ProductID == productID, trackChanges).FirstOrDefaultAsync(cancellationToken);
        if (product is not null)
            return product;
        else
            return null;
    }

    public void CreateEntity(ProductModel entity, bool trackChanges)
    {
        Create(entity);
    }

    public void UpdateEntity(ProductModel entity, bool trackChanges)
    {
        Update(entity);
    }

    public void DeleteEntity(ProductModel entity, bool trackChanges)
    {
        Delete(entity);
    }
}
