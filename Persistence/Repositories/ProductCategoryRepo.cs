using Application.IRepositories;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.EntityFrameworkCore;
using Persistence.Context;
using Persistence.Extensions;

namespace Persistence.Repositories;

public sealed class ProductCategoryRepo : MethodBase<ProductCategoryModel>, IProductCategoryRepo
{
    public ProductCategoryRepo(AppDBContext dbContext) : base(dbContext) { }

    public async Task<PagedList<ProductCategoryModel>> GetAllAsync(ProductCategoryParam productCategoryParam, bool trackChanges)
    {
        var productCategories = await FindAll(trackChanges)
            .Skip((productCategoryParam.PageNumber - 1) * productCategoryParam.PageSize)
            .Take(productCategoryParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges).CountAsync();

        return new PagedList<ProductCategoryModel>(productCategories, count, productCategoryParam.PageNumber, productCategoryParam.PageSize);
    }

    public async Task<PagedList<ProductCategoryModel>> GetByParametersAsync(ProductCategoryParam productCategoryParam, bool trackChanges)
    {
        var productCategories = await FindAll(trackChanges)
            .SearchByName(productCategoryParam.srcByName) //It's a local method
            .Skip((productCategoryParam.PageNumber - 1) * productCategoryParam.PageSize)
            .Take(productCategoryParam.PageSize)
            .ToListAsync();

        var count = await FindAll(trackChanges)
            .SearchByName(productCategoryParam.srcByName)
            .CountAsync();

        return new PagedList<ProductCategoryModel>(productCategories, count, productCategoryParam.PageNumber, productCategoryParam.PageSize);
    }

    public async Task<ProductCategoryModel?> GetByIDAsync(int categoryID, bool trackChanges)
    {
        var productCategory = await FindByCondition(x => x.CategoryID == categoryID, trackChanges).FirstOrDefaultAsync();
        if (productCategory is not null)
            return productCategory;
        else
            return null;
    }

    public void CreateEntity(ProductCategoryModel entity, bool trackChanges)
    {
        Create(entity);
    }

    public void UpdateEntity(ProductCategoryModel entity, bool trackChanges)
    {
        Update(entity);
    }

    public void DeleteEntity(ProductCategoryModel entity, bool trackChanges)
    {
        Delete(entity);
    }
}
