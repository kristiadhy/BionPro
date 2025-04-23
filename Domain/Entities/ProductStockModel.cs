namespace Domain.Entities;
public class ProductStockModel : BaseEntity
{
  public int? StockId { get; set; }
  public int Quantity { get; set; }

  public Guid? ProductID { get; set; }
  public ProductModel? Product { get; set; }
  public int? LocationID { get; set; }
  public ProductLocationModel Location { get; set; }
}
