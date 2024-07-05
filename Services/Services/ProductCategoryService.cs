using Application.Exceptions;
using Application.IRepositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Serilog;
using Services.Contracts.IServices;

namespace Services.Services;
public class ProductCategoryService : IProductCategoryService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    public ProductCategoryService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<ProductCategoryDto> productCategoryDto, MetaData metaData)> GetByParametersAsync(int categoryID, ProductCategoryParam productCategoryParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get product categories");

        var productCategories = await _repositoryManager.ProductCategoryRepo.GetByParametersAsync(productCategoryParam, trackChanges, cancellationToken);

        _logger.Information("Product categories retrieved");

        var productCategoriesToReturn = _mapper.Map<IEnumerable<ProductCategoryDto>>(productCategories);

        return (productCategoriesToReturn, productCategories.MetaData);
    }

    public async Task<ProductCategoryDto> GetByProductCategoryIDAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get product category with ID : {categoryID}", categoryID);

        var productCategory = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, trackChanges, cancellationToken);
        if (productCategory is null)
            throw new ProductCategoryNotFoundException(categoryID);

        _logger.Information("Product category {categoryName} retrieved", productCategory.Name);

        var productCategoryToReturn = _mapper.Map<ProductCategoryDto>(productCategory);
        return productCategoryToReturn;
    }

    public async Task<ProductCategoryDto> CreateAsync(ProductCategoryDto productCategoryDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryModel = _mapper.Map<ProductCategoryModel>(productCategoryDto);
        var validator = new ProductCategoryValidator();
        validator.ValidateInput(productCategoryModel);

        _logger.Information("Insert new product category {productCategoryName}", productCategoryDto.Name);

        _repositoryManager.ProductCategoryRepo.CreateEntity(productCategoryModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product category {productCategory} added", productCategoryModel.Name);

        var productCategoriesToReturn = _mapper.Map<ProductCategoryDto>(productCategoryModel);

        return productCategoriesToReturn;
    }

    public async Task UpdateAsync(ProductCategoryDto productCategoryDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryToUpdate = _mapper.Map<ProductCategoryModel>(productCategoryDto);
        var validator = new ProductCategoryValidator();
        validator.ValidateInput(productCategoryToUpdate);

        _logger.Information("Update product category {productCategoryName}", productCategoryDto.Name);

        _repositoryManager.ProductCategoryRepo.UpdateEntity(productCategoryToUpdate, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product category {productCategory} updated", productCategoryToUpdate.Name);
    }

    public async Task DeleteAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryToDelete = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, trackChanges, cancellationToken);
        if (productCategoryToDelete is null)
            throw new ProductCategoryNotFoundException(categoryID);

        _logger.Information("Delete product Category {productCategoryName}", productCategoryToDelete.Name);

        _repositoryManager.ProductCategoryRepo.DeleteEntity(productCategoryToDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product category {productCategory} deleted", productCategoryToDelete.Name);
    }

    public async Task<(ProductCategoryDto productCategoryToPatch, ProductCategoryModel productCategory)> GetProductCategoryForPatchAsync(int categoryID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get product category with ID : {categoryID}", categoryID);

        var productCategory = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, empTrackChanges, cancellationToken);
        if (productCategory is null)
            throw new ProductCategoryNotFoundException(categoryID);

        _logger.Information("Product category {categoryName} retrieved", productCategory.Name);

        var productCategoriesToPatch = _mapper.Map<ProductCategoryDto>(productCategory);

        return (productCategoriesToPatch, productCategory);
    }

    public async Task SaveChangesForPatchAsync(ProductCategoryDto productCategoryDto, ProductCategoryModel productCategoryModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(productCategoryDto, productCategoryModel);

        _logger.Information("Update product category {productCategoryName}", productCategoryDto.Name);

        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product category {productCategory} updated", productCategoryDto.Name);
    }
}
