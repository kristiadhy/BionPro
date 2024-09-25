using Domain.DTO;

namespace WebAssembly.State;

public class SaleInputState
{
    public SaleDto SaleForTransaction { get; set; } = new();
}
