using Domain.DTO;

namespace WebAssembly.State;

public class CustomerInputState()
{
    public CustomerDTO Customer { get; set; } = new();
}
