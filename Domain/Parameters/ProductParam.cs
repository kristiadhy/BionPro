namespace Domain.Parameters;
public class ProductParam : RequestParameters
{
  public ProductParam() => OrderBy = "Name";
  public int? SrcByProductCategory { get; set; }
  public string? SrcByProductName { get; set; }
}
