using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;
namespace Web.Services.IHttpRepository;

public interface ISupplierHttpService
{
    public Task<PagingResponse<SupplierDto>> GetSuppliers(SupplierParam supplierParam);
    public Task<SupplierDto> GetSupplierByID(Guid supplierID);
    public Task<HttpResponseMessage> Create(SupplierDto supplierDto);
    public Task<HttpResponseMessage> Update(SupplierDto supplierDto);
    public Task<HttpResponseMessage> Delete(Guid supplierID);
}
