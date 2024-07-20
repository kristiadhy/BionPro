namespace Domain.Parameters;
public class SaleParam : RequestParameters
{
    public SaleParam() => OrderBy = "Date";
    public Guid? SrcCustomerID { get; set; }
    public string? SrcCustomerName { get; set; }
    public DateTimeOffset? SrcDateFrom { get; set; }
    public DateTimeOffset? SrcDateTo { get; set; }
}
