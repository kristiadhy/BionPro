namespace Domain.Parameters;
public class PurchaseDetailParam : RequestParameters
{
    public PurchaseDetailParam() => OrderBy = "ProductName";
    public string? SrcProduct { get; set; }
}
