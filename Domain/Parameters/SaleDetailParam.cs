namespace Domain.Parameters;
public class SaleDetailParam : RequestParameters
{
  public SaleDetailParam() => OrderBy = "ProductName";
  public string? SrcProduct { get; set; }
}
