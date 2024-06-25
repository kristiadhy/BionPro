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
        _logger.Information($"Get product categories");

        var productCategories = await _repositoryManager.ProductCategoryRepo.GetByParametersAsync(productCategoryParam, trackChanges);

        var productCategoriesToReturn = _mapper.Map<IEnumerable<ProductCategoryDto>>(productCategories);

        return (productCategoriesToReturn, productCategories.MetaData);
    }

    public async Task<ProductCategoryDto> GetByProductCategoryIDAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get product category with ID : {categoryID}");

        var productCategories = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, trackChanges);
        if (productCategories is null)
            throw new ProductCategoryNotFoundException(categoryID);

        var productCategoriesToReturn = _mapper.Map<ProductCategoryDto>(productCategories);
        return productCategoriesToReturn;
    }

    public async Task<ProductCategoryDto> CreateAsync(ProductCategoryDto entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryForValidation = _mapper.Map<ProductCategoryModel>(entity);
        var validator = new ProductCategoryValidator();
        validator.ValidateInput(productCategoryForValidation);

        _logger.Information("Insert new product category : {productCategoryName}", entity.Name);

        var productCategoryModel = _mapper.Map<ProductCategoryModel>(entity);
        _repositoryManager.ProductCategoryRepo.CreateEntity(productCategoryModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var productCategoriesToReturn = _mapper.Map<ProductCategoryDto>(productCategoryModel);

        return productCategoriesToReturn;
    }

    public async Task UpdateAsync(ProductCategoryDto entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryForValidation = _mapper.Map<ProductCategoryModel>(entity);
        var validator = new ProductCategoryValidator();
        validator.ValidateInput(productCategoryForValidation);

        _logger.Information("Update product category : {productCategoryName}", entity.Name);

        ProductCategoryModel productCategoryToUpdate = _mapper.Map<ProductCategoryModel>(entity);
        _repositoryManager.ProductCategoryRepo.UpdateEntity(productCategoryToUpdate, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryForDelete = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, trackChanges);
        if (productCategoryForDelete is null)
            throw new ProductCategoryNotFoundException(categoryID);

        _logger.Information("Delete product Category : {productCategoryName}", productCategoryForDelete.Name);

        _repositoryManager.ProductCategoryRepo.DeleteEntity(productCategoryForDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(ProductCategoryDto productCategoryToPatch, ProductCategoryModel productCategory)> GetProductCategoryForPatchAsync(int categoryID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var productCategories = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, empTrackChanges);
        if (productCategories is null)
            throw new ProductCategoryNotFoundException(categoryID);

        var productCategoriesToPatch = _mapper.Map<ProductCategoryDto>(productCategories);

        return (productCategoriesToPatch, productCategories);
    }

    public async Task SaveChangesForPatchAsync(ProductCategoryDto productCategoryDto, ProductCategoryModel productCategoryModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(productCategoryDto, productCategoryModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }
}
