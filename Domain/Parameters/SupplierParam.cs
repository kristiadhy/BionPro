namespace Domain.Parameters;

public class SupplierParam : RequestParameters
{
  public SupplierParam() => OrderBy = "SupplierName";
  public string? SrcByName { get; set; }
}