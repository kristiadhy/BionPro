namespace Domain.Parameters;
public class ProductCategoryParam : RequestParameters
{
    public ProductCategoryParam() => OrderBy = "Name";
    public string? SrcByName { get; set; }
}
