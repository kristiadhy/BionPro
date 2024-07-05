using Application.Exceptions;
using Application.IRepositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Microsoft.AspNetCore.Http;
using Serilog;
using Services.Contracts.IServices;
using System.Net.Http.Headers;

namespace Services.Services;
public class ProductService : IProductService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    private readonly string folderPathOnServer = Path.Combine("StaticFiles", "Images", "Products");
    private readonly string folderUrlToServer = "StaticFiles/Images/Products";

    public ProductService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<ProductDtoForProductQueries> productDto, MetaData metaData)> GetByParametersAsync(Guid productID, ProductParam productParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get products");

        var products = await _repositoryManager.ProductRepo.GetByParametersAsync(productParam, trackChanges, cancellationToken);

        _logger.Information("Products retrieved");

        var productCategoriesToReturn = _mapper.Map<IEnumerable<ProductDtoForProductQueries>>(products);

        return (productCategoriesToReturn, products.MetaData);
    }

    public async Task<ProductDto> GetByProductIDAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get product with ID : {productID}");

        var product = await _repositoryManager.ProductRepo.GetByIDAsync(productID, trackChanges, cancellationToken);
        if (product is null)
            throw new ProductNotFoundException(productID);

        _logger.Information("Product {productName} retrieved", product.Name);

        var productCategoriesToReturn = _mapper.Map<ProductDto>(product);
        return productCategoriesToReturn;
    }

    public async Task<ProductDto> CreateAsync(ProductDto productDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productModel = _mapper.Map<ProductModel>(productDto);
        var validator = new ProductValidator();
        validator.ValidateInput(productModel);

        _logger.Information("Insert new product {productName}", productDto.Name);

        _repositoryManager.ProductRepo.CreateEntity(productModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product {productName} added", productModel.Name);

        var productCategoriesToReturn = _mapper.Map<ProductDto>(productModel);

        return productCategoriesToReturn;
    }

    public async Task UpdateAsync(ProductDto productDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productModel = _mapper.Map<ProductModel>(productDto);
        var validator = new ProductValidator();
        validator.ValidateInput(productModel);

        _logger.Information("Update product {productName}", productDto.Name);

        _repositoryManager.ProductRepo.UpdateEntity(productModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product {productName} updated", productModel.Name);
    }

    public async Task DeleteAsync(Guid productID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var productToDelete = await _repositoryManager.ProductRepo.GetByIDAsync(productID, trackChanges, cancellationToken);
        if (productToDelete is null)
            throw new ProductNotFoundException(productID);

        _logger.Information("Delete product {productName}", productToDelete.Name);

        _repositoryManager.ProductRepo.DeleteEntity(productToDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Product {productName} deleted", productToDelete.Name);
    }

    public async Task<(ProductDto productToPatch, ProductModel product)> GetProductForPatchAsync(Guid productID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var productCategories = await _repositoryManager.ProductRepo.GetByIDAsync(productID, empTrackChanges, cancellationToken) ?? throw new ProductNotFoundException(productID);
        var productCategoriesToPatch = _mapper.Map<ProductDto>(productCategories);

        return (productCategoriesToPatch, productCategories);
    }

    public async Task SaveChangesForPatchAsync(ProductDto productDto, ProductModel productModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(productDto, productModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<byte[]?> GetProductImage(string fileName, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get product image {productImage}", fileName);

        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderPathOnServer, fileName);

        if (File.Exists(fullPath))
        {
            var fileBytes = await File.ReadAllBytesAsync(fullPath, cancellationToken);
            _logger.Information("Product image {productImage} retrieved", fileName);
            return fileBytes;
        }
        else
            return null;
    }

    public async Task<string?> UploadProductImage(IFormFile file, CancellationToken cancellationToken = default)
    {
        if (file.Length == 0)
            return null;

        var fullPathToSave = Path.Combine(Directory.GetCurrentDirectory(), folderPathOnServer);
        var originalFileName = ContentDispositionHeaderValue.Parse(file.ContentDisposition)!.FileName!.Trim('"');
        var fileName = $"{Guid.NewGuid()}-{originalFileName}"; // Ensure unique file name
        var fullImagePathToSave = Path.Combine(fullPathToSave, fileName);

        try
        {
            Directory.CreateDirectory(fullPathToSave); // Ensure the directory exists

            await using (var stream = new FileStream(fullImagePathToSave, FileMode.Create))
                await file.CopyToAsync(stream, cancellationToken);

            _logger.Information("Product image {productImage} have been copied to the server", fileName);

            var imageUrl = $"{folderUrlToServer}/{fileName}";

            return imageUrl;
        }
        catch (Exception ex)
        {
            _logger.Error($"An error occurred while uploading the image: {ex.Message}");
            return null;
        }
    }

    public async Task DeleteProductImage(string fileName, CancellationToken cancellationToken = default)
    {
        var fullPath = Path.Combine(Directory.GetCurrentDirectory(), folderPathOnServer, fileName);

        if (!File.Exists(fullPath))
            throw new PathNotFoundException();

        await Task.Run(() => File.Delete(fullPath), cancellationToken);

        _logger.Information("Product image {FileName} deleted", fileName);
    }

}
