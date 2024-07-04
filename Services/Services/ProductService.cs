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
public class ProductService : IProductService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    public ProductService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<ProductDtoForProductQueries> productDto, MetaData metaData)> GetByParametersAsync(Guid productID, ProductParam productParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get product");

        var productCategories = await _repositoryManager.ProductRepo.GetByParametersAsync(productParam, trackChanges);

        var productCategoriesToReturn = _mapper.Map<IEnumerable<ProductDtoForProductQueries>>(productCategories);

        return (productCategoriesToReturn, productCategories.MetaData);
    }

    public async Task<ProductDto> GetByProductIDAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get product with ID : {productID}");

        var productCategories = await _repositoryManager.ProductRepo.GetByIDAsync(productID, trackChanges);
        if (productCategories is null)
            throw new ProductNotFoundException(productID);

        var productCategoriesToReturn = _mapper.Map<ProductDto>(productCategories);
        return productCategoriesToReturn;
    }

    public async Task<ProductDto> CreateAsync(ProductDto entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productForValidation = _mapper.Map<ProductModel>(entity);
        var validator = new ProductValidator();
        validator.ValidateInput(productForValidation);

        _logger.Information("Insert new product : {productName}", entity.Name);

        var productModel = _mapper.Map<ProductModel>(entity);
        _repositoryManager.ProductRepo.CreateEntity(productModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var productCategoriesToReturn = _mapper.Map<ProductDto>(productModel);

        return productCategoriesToReturn;
    }

    public async Task UpdateAsync(ProductDto entity, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productForValidation = _mapper.Map<ProductModel>(entity);
        var validator = new ProductValidator();
        validator.ValidateInput(productForValidation);

        _logger.Information("Update product : {productName}", entity.Name);

        ProductModel productToUpdate = _mapper.Map<ProductModel>(entity);
        _repositoryManager.ProductRepo.UpdateEntity(productToUpdate, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productForDelete = await _repositoryManager.ProductRepo.GetByIDAsync(productID, trackChanges);
        if (productForDelete is null)
            throw new ProductNotFoundException(productID);

        _logger.Information("Delete product  : {productName}", productForDelete.Name);

        _repositoryManager.ProductRepo.DeleteEntity(productForDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(ProductDto productToPatch, ProductModel product)> GetProductForPatchAsync(Guid productID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var productCategories = await _repositoryManager.ProductRepo.GetByIDAsync(productID, empTrackChanges);
        if (productCategories is null)
            throw new ProductNotFoundException(productID);

        var productCategoriesToPatch = _mapper.Map<ProductDto>(productCategories);

        return (productCategoriesToPatch, productCategories);
    }

    public async Task SaveChangesForPatchAsync(ProductDto productDto, ProductModel productModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(productDto, productModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }
}
