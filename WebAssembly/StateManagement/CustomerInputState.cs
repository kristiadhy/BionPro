using Domain.DTO;
using Domain.Parameters;
using Web.Services.IHttpRepository;

namespace WebAssembly.StateManagement;

public class CustomerInputState()
{
    public CustomerDTO Customer { get; set; } = new();
}
