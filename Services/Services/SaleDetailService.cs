using Application.IRepositories;
using AutoMapper;
using Domain.DTO;
using Domain.Parameters;
using Serilog;
using Services.Contracts.IServices;

namespace Services.Services;
internal class SaleDetailService : ISaleDetailService
{
  private readonly IRepositoryManager _repositoryManager;
  private readonly IMapper _mapper;
  private readonly ILogger _logger;

  public SaleDetailService(IRepositoryManager repositoryManager, IMapper mapper, ILogger logger)
  {
    _repositoryManager = repositoryManager;
    _mapper = mapper;
    _logger = logger;
  }

  public async Task<(IEnumerable<SaleDetailDto> saleDetailDto, MetaData metaData)> GetBySaleDetailByIDAsync(int saleID, SaleDetailParam saleDetailParam, bool trackChanges, CancellationToken cancellationToken = default)
  {
    var saleDetails = await _repositoryManager.SaleDetailRepo.GetByIDAsync(saleID, saleDetailParam, trackChanges, cancellationToken);
    var saleDetailsToReturn = _mapper.Map<IEnumerable<SaleDetailDto>>(saleDetails);

    return (saleDetailsToReturn, saleDetails.MetaData);
  }
}
