namespace Domain.Entities;
public class ProductStockModel : BaseEntity
{
    public int StockId { get; set; }
    public Guid ProductID { get; set; }
    public int LocationID { get; set; }
    public int Quantity { get; set; }

    public ProductModel Product { get; set; } = new();
    public ProductLocationModel Location { get; set; } = new();
}
