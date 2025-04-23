using Domain.DTO;
using Domain.Parameters;
using Web.Services.Features;

namespace Web.Services.IHttpRepository;

public interface ICustomerHttpService
{
  public Task<PagingResponse<CustomerDTO>> GetCustomers(CustomerParam customerParameter);
  public Task<CustomerDTO> GetCustomerByID(Guid customerID);
  public Task Create(CustomerDTO customerDTO);
  public Task Update(CustomerDTO customerDTO);
  public Task Delete(Guid customerID);
}
