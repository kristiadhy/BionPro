using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;
namespace Web.Services.IHttpRepository;

public interface ISupplierHttpService
{
  public Task<PagingResponse<SupplierDto>> GetSuppliers(SupplierParam supplierParam);
  public Task<SupplierDto> GetSupplierByID(Guid supplierID);
  public Task Create(SupplierDto supplierDto);
  public Task Update(SupplierDto supplierDto);
  public Task Delete(Guid supplierID);
}
