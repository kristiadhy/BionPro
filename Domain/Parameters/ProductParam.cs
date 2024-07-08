namespace Domain.Parameters;
public class ProductParam : RequestParameters
{
    public ProductParam() => OrderBy = "Name";
    public string? SrcByName { get; set; }
}
