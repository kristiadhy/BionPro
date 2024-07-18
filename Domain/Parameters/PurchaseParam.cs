namespace Domain.Parameters;
public class PurchaseParam : RequestParameters
{
    public PurchaseParam() => OrderBy = "Date";
    public Guid? SrcSupplierID { get; set; }
    public string? SrcSupplierName { get; set; }
    public DateTimeOffset? SrcDateFrom { get; set; }
    public DateTimeOffset? SrcDateTo { get; set; }
}
