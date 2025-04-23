namespace Domain.Entities;
public class ProductLocationModel : BaseEntity
{
  public int? LocationID { get; set; }
  public string? Name { get; set; }
  public string? Description { get; set; }
  public ICollection<ProductStockModel>? Stocks { get; set; }
}
