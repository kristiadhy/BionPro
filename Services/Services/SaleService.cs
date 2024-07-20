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

namespace Services.Services;
internal sealed class SaleService : ISaleService
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;

    public SaleService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
    }

    public async Task<(IEnumerable<SaleDtoForSummary> saleDto, MetaData metaData)> GetSummaryByParametersAsync(int saleID, SaleParam saleParam, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var sales = await _repositoryManager.SaleRepo.GetSummaryByParametersAsync(saleParam, trackChanges, cancellationToken);
        return (sales, sales.MetaData);
    }

    public async Task<SaleDto> GetBySaleIDAsync(int saleID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var sale = await _repositoryManager.SaleRepo.GetByIDAsync(saleID, trackChanges, cancellationToken);
        if (sale is null)
            throw new SaleIDNotFoundException(saleID);

        var saleToReturn = _mapper.Map<SaleDto>(sale);

        return saleToReturn;
    }

    public async Task<SaleDto> CreateAsync(SaleDto dto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        //Just in case user just edit the data and then save a new data, the SaleID will already exist. Hence, we need to reset it.
        if (dto.SaleID is not null)
            dto.SaleID = null;

        //Generate unique transaction code for sale before saving to database
        dto.TransactionCode = await GenerateUniqueTransactionCode(trackChanges, cancellationToken);

        var saleModel = _mapper.Map<SaleModel>(dto);
        var validator = new SaleValidator();
        validator.ValidateInput(saleModel);

        _repositoryManager.SaleRepo.CreateEntity(saleModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);

        var saleToReturn = _mapper.Map<SaleDto>(saleModel);

        return saleToReturn;
    }

    public async Task UpdateAsync(SaleDto dto, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var saleModel = _mapper.Map<SaleModel>(dto);
        var validator = new SaleValidator();
        validator.ValidateInput(saleModel);

        _repositoryManager.SaleRepo.UpdateEntity(saleModel, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task DeleteAsync(int saleID, bool trackChanges, CancellationToken cancellationToken = default)
    {
        var saleToDelete = await _repositoryManager.SaleRepo.GetByIDAsync(saleID, trackChanges, cancellationToken);
        if (saleToDelete is null)
            throw new SaleIDNotFoundException(saleID);

        _repositoryManager.SaleRepo.DeleteEntity(saleToDelete, trackChanges);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    public async Task<(SaleDto saleToPatch, SaleModel sale)> GetSaleForPatchAsync(int saleID, bool empTrackChanges, CancellationToken cancellationToken = default)
    {
        var sales = await _repositoryManager.SaleRepo.GetByIDAsync(saleID, empTrackChanges, cancellationToken) ?? throw new SaleIDNotFoundException(saleID);
        var salesToPatch = _mapper.Map<SaleDto>(sales);

        return (salesToPatch, sales);
    }

    public async Task SaveChangesForPatchAsync(SaleDto saleDto, SaleModel saleModel, CancellationToken cancellationToken = default)
    {
        _mapper.Map(saleDto, saleModel);
        await _repositoryManager.UnitOfWorkRepo.SaveChangesAsync(cancellationToken);
    }

    private async Task<string> GenerateUniqueTransactionCode(bool trackChanges, CancellationToken cancellationToken)
    {
        var prefix = GlobalConstant.TransactionCodes.Where(s => s.ID == 2).FirstOrDefault()?.Prefix ?? string.Empty;
        var transactionCode = TransactionCodeFactory.GenerateTransactionCode(prefix);
        var isExistTransactionCode = await _repositoryManager.SaleRepo.CheckTransactionCodeExistAsync(transactionCode, trackChanges, cancellationToken);

        if (isExistTransactionCode)
        {
            // If transaction code already exists, recursively call this method until a unique code is generated.
            return await GenerateUniqueTransactionCode(trackChanges, cancellationToken);
        }

        return transactionCode;
    }
}
