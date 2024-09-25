using Domain.DTO;

namespace WebAssembly.State;

public class PurchaseInputState
{
    public PurchaseDto PurchaseForTransaction { get; set; } = new();
}
