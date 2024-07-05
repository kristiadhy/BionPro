using Application.Exceptions;
using Application.IRepositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Serilog;
using Services.Contracts;

namespace Services;

internal sealed class SupplierService : ISupplierService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SupplierService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<SupplierDto> supplierDto, MetaData metaData)> GetByParametersAsync(Guid supplierID, SupplierParam supplierParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get suppliers");

        var suppliers = await _repositoryManager.SupplierRepo.GetByParametersAsync(supplierParam, trackChanges, cancellationToken);

        _logger.Information("Suppliers retrieved");

        var suppliersToReturn = _mapper.Map<IEnumerable<SupplierDto>>(suppliers);

        return (suppliersToReturn, suppliers.MetaData);
    }

    public async Task<SupplierDto> GetBySupplierIDAsync(Guid supplierID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information($"Get supplier with ID : {supplierID}");

        var suppliers = await _repositoryManager.SupplierRepo.GetByIDAsync(supplierID, trackChanges, cancellationToken);
        if (suppliers is null)
            throw new SupplierIDNotFoundException(supplierID);

        _logger.Information("Supplier {supplierName} retrieved", suppliers.SupplierName);

        var suppliersToReturn = _mapper.Map<SupplierDto>(suppliers);
        return suppliersToReturn;
    }

    public async Task<SupplierDto> CreateAsync(SupplierDto supplierDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var supplierModel = _mapper.Map<SupplierModel>(supplierDto);
        var validator = new SupplierValidator();
        validator.ValidateInput(supplierModel);

        _logger.Information("Insert new supplier {supplierName}", supplierDto.SupplierName);

        _repositoryManager.SupplierRepo.CreateEntity(supplierModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Supplier {supplierName} added", supplierDto.SupplierName);

        var suppliersToReturn = _mapper.Map<SupplierDto>(supplierModel);

        return suppliersToReturn;
    }

    public async Task UpdateAsync(SupplierDto supplierDto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var suppliersToUpdate = _mapper.Map<SupplierModel>(supplierDto);
        var validator = new SupplierValidator();
        validator.ValidateInput(suppliersToUpdate);

        _logger.Information("Update supplier {suppliersName}", supplierDto.SupplierName);

        _repositoryManager.SupplierRepo.UpdateEntity(suppliersToUpdate, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Supplier {supplierName} updated", supplierDto.SupplierName);
    }

    public async Task DeleteAsync(Guid suppliersID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var supplierToDelete = await _repositoryManager.SupplierRepo.GetByIDAsync(suppliersID, trackChanges, cancellationToken);
        if (supplierToDelete is null)
            throw new SupplierIDNotFoundException(suppliersID);

        _logger.Information("Delete suppliers : {supplierName}", supplierToDelete.SupplierName);

        _repositoryManager.SupplierRepo.DeleteEntity(supplierToDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Supplier {supplierName} deleted", supplierToDelete.SupplierName);
    }

    public async Task<(SupplierDto supplierToPatch, SupplierModel supplier)> GetSupplierForPatchAsync(Guid supplierID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        _logger.Information("Get supplier with ID : {supplierID}", supplierID);

        var supplier = await _repositoryManager.SupplierRepo.GetByIDAsync(supplierID, empTrackChanges, cancellationToken);
        if (supplier is null)
            throw new SupplierIDNotFoundException(supplierID);

        _logger.Information("Supplier {supplierName} retrieved", supplier.SupplierName);

        var supplierToPatch = _mapper.Map<SupplierDto>(supplier);

        return (supplierToPatch, supplier);
    }

    public async Task SaveChangesForPatchAsync(SupplierDto supplierToPatch, SupplierModel supplier, CancellationToken cancellationToken = default)
    {
        _mapper.Map(supplierToPatch, supplier);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        _logger.Information("Supplier {supplierName} updated", supplierToPatch.SupplierName);
    }
}
