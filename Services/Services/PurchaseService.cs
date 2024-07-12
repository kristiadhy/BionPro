using Application.Constants;
using Application.Exceptions;
using Application.Factories;
using Application.IRepositories;
using Application.Validators;
using AutoMapper;
using Domain.DTO;
using Domain.Entities;
using Domain.Parameters;
using Serilog;
using Services.Contracts.IServices;
using System.Transactions;

namespace Services.Services;
internal sealed class PurchaseService : IPurchaseService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public PurchaseService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<PurchaseDtoForSummary> purchaseDto, MetaData metaData)> GetSummaryByParametersAsync(int purchaseID, PurchaseParam purchaseParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await _repositoryManager.PurchaseRepo.GetSummaryByParametersAsync(purchaseParam, trackChanges, cancellationToken);
        return (purchases, purchases.MetaData);
    }

    public async Task<PurchaseDto> GetByPurchaseIDAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchase = await _repositoryManager.PurchaseRepo.GetByIDAsync(purchaseID, trackChanges, cancellationToken);
        if (purchase is null)
            throw new PurchaseIDNotFoundException(purchaseID);

        var purchaseToReturn = _mapper.Map<PurchaseDto>(purchase);

        return purchaseToReturn;
    }

    public async Task<PurchaseDto> CreateAsync(PurchaseDto dto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //Just in case user just edit the data and then save a new data, the PurchaseID will already exist. Hence, we need to reset it.
        if (dto.PurchaseID != 0)
            dto.PurchaseID = 0;

        //Generate unique transaction code for purchase before saving to database
        dto.TransactionCode = await GenerateUniqueTransactionCode(trackChanges, cancellationToken);

        var purchaseModel = _mapper.Map<PurchaseModel>(dto);
        var validator = new PurchaseValidator();
        validator.ValidateInput(purchaseModel);

        _repositoryManager.PurchaseRepo.CreateEntity(purchaseModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var purchaseToReturn = _mapper.Map<PurchaseDto>(purchaseModel);

        return purchaseToReturn;
    }

    public async Task UpdateAsync(PurchaseDto dto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchaseModel = _mapper.Map<PurchaseModel>(dto);
        var validator = new PurchaseValidator();
        validator.ValidateInput(purchaseModel);

        _repositoryManager.PurchaseRepo.UpdateEntity(purchaseModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int purchaseID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var purchaseToDelete = await _repositoryManager.PurchaseRepo.GetByIDAsync(purchaseID, trackChanges, cancellationToken);
        if (purchaseToDelete is null)
            throw new PurchaseIDNotFoundException(purchaseID);

        _repositoryManager.PurchaseRepo.DeleteEntity(purchaseToDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(PurchaseDto purchaseToPatch, PurchaseModel purchase)> GetPurchaseForPatchAsync(int purchaseID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var purchases = await _repositoryManager.PurchaseRepo.GetByIDAsync(purchaseID, empTrackChanges, cancellationToken) ?? throw new PurchaseIDNotFoundException(purchaseID);
        var purchasesToPatch = _mapper.Map<PurchaseDto>(purchases);

        return (purchasesToPatch, purchases);
    }

    public async Task SaveChangesForPatchAsync(PurchaseDto purchaseDto, PurchaseModel purchaseModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(purchaseDto, purchaseModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GenerateUniqueTransactionCode(bool trackChanges, CancellationToken cancellationToken)
    {
        var prefix = GlobalConstant.TransactionCodes.Where(s => s.ID == 1).FirstOrDefault()?.Prefix ?? string.Empty;
        var transactionCode = TransactionCodeFactory.GenerateTransactionCode(prefix);
        var isExistTransactionCode = await _repositoryManager.PurchaseRepo.CheckTransactionCodeExistAsync(transactionCode, trackChanges, cancellationToken);

        if (isExistTransactionCode)
        {
            // If transaction code already exists, recursively call this method until a unique code is generated.
            return await GenerateUniqueTransactionCode(trackChanges, cancellationToken);
        }

        return transactionCode;
    }
}
