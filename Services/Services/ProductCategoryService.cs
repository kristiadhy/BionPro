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

        var productCategories = await _repositoryManager.ProductCategoryRepo.GetByParametersAsync(productCategoryParam, trackChanges, cancellationToken);

        var productCategoriesToReturn = _mapper.Map<IEnumerable<ProductCategoryDto>>(productCategories);

        return (productCategoriesToReturn, productCategories.MetaData);
    }

    public async Task<ProductCategoryDto> GetByProductCategoryIDAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategory = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, trackChanges, cancellationToken);
        if (productCategory is null)
            throw new ProductCategoryNotFoundException(categoryID);

        var productCategoryToReturn = _mapper.Map<ProductCategoryDto>(productCategory);
        return productCategoryToReturn;
    }

    public async Task<ProductCategoryDto> CreateAsync(ProductCategoryDto productCategoryDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryModel = _mapper.Map<ProductCategoryModel>(productCategoryDto);
        var validator = new ProductCategoryValidator();
        validator.ValidateInput(productCategoryModel);

        productCategoryModel.SetDataCreate();
        _repositoryManager.ProductCategoryRepo.CreateEntity(productCategoryModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var productCategoriesToReturn = _mapper.Map<ProductCategoryDto>(productCategoryModel);

        return productCategoriesToReturn;
    }

    public async Task UpdateAsync(ProductCategoryDto productCategoryDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryToUpdate = _mapper.Map<ProductCategoryModel>(productCategoryDto);
        var validator = new ProductCategoryValidator();
        validator.ValidateInput(productCategoryToUpdate);

        productCategoryToUpdate.SetDataUpdate();
        _repositoryManager.ProductCategoryRepo.UpdateEntity(productCategoryToUpdate);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int categoryID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productCategoryToDelete = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, trackChanges, cancellationToken);
        if (productCategoryToDelete is null)
            throw new ProductCategoryNotFoundException(categoryID);

        _repositoryManager.ProductCategoryRepo.DeleteEntity(productCategoryToDelete);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(ProductCategoryDto productCategoryToPatch, ProductCategoryModel productCategory)> GetProductCategoryForPatchAsync(int categoryID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var productCategory = await _repositoryManager.ProductCategoryRepo.GetByIDAsync(categoryID, empTrackChanges, cancellationToken);
        if (productCategory is null)
            throw new ProductCategoryNotFoundException(categoryID);

        var productCategoriesToPatch = _mapper.Map<ProductCategoryDto>(productCategory);

        return (productCategoriesToPatch, productCategory);
    }

    public async Task SaveChangesForPatchAsync(ProductCategoryDto productCategoryDto, ProductCategoryModel productCategoryModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(productCategoryDto, productCategoryModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }
}
