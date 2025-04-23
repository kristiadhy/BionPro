using Application.IRepositories;
using AutoMapper;
using Domain.DTO;
using Domain.Parameters;
using Serilog;
using Services.Contracts.IServices;

namespace Services.Services;
internal class PurchaseDetailService : IPurchaseDetailService
{
  private readonly IRepositoryManager _repositoryManager;
  private readonly IMapper _mapper;
  private readonly ILogger _logger;

  public PurchaseDetailService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
  {
    _repositoryManager = repositoryManager;
    _mapper = mapper;
    _logger = logger;
  }

  public async Task<(IEnumerable<PurchaseDetailDto> purchaseDetailDto, MetaData metaData)> GetByPurchaseDetailByIDAsync(int purchaseID, PurchaseDetailParam purchaseDetailParam, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var purchaseDetails = await _repositoryManager.PurchaseDetailRepo.GetByIDAsync(purchaseID, purchaseDetailParam, trackChanges, cancellationToken);
    var purchaseDetailsToReturn = _mapper.Map<IEnumerable<PurchaseDetailDto>>(purchaseDetails);

    return (purchaseDetailsToReturn, purchaseDetails.MetaData);
  }
}
