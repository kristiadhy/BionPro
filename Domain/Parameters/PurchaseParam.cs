namespace Domain.Parameters;
public class PurchaseParam : RequestParameters
{
    public PurchaseParam() => OrderBy = "Date";
    public string? SrcSupplier { get; set; }
    public DateTimeOffset? SrcDateFrom { get; set; }
    public DateTimeOffset? SrcDateTo { get; set; }
}
