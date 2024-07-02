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

    public async Task<PagedList<ProductModel>> GetAllAsync(ProductParam productParam, bool trackChanges)
    {
        var productCategories = await FindAll(trackChanges)
            .Skip((productParam.PageNumber - 1) * productParam.PageSize)
            .Take(productParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<ProductModel>(productCategories, count, productParam.PageNumber, productParam.PageSize);
    }

    public async Task<PagedList<ProductModel>> GetByParametersAsync(ProductParam productParam, bool trackChanges)
    {
        var productCategories = await FindAll(trackChanges)
            .SearchByName(productParam.srcByName) //It's a local method
            .Skip((productParam.PageNumber - 1) * productParam.PageSize)
            .Take(productParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .SearchByName(productParam.srcByName)
            .CountAsync();

        return new PagedList<ProductModel>(productCategories, count, productParam.PageNumber, productParam.PageSize);
    }

    public async Task<ProductModel?> GetByIDAsync(Guid productID, bool trackChanges)
    {
        var product = await FindByCondition(x => x.ProductID == productID, trackChanges).FirstOrDefaultAsync();
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
