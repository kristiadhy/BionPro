namespace Domain.DTO;
public class SaleDetailDto
{
  public int Quantity { get; set; }
  public decimal Price { get; set; }
  public decimal DiscountPercentage { get; set; }
  public decimal DiscountAmount { get; set; }
  public decimal SubTotal => (Quantity * Price) - (DiscountPercentage * Price * Quantity);
  public int? SaleID { get; set; }
  public Guid? ProductID { get; set; }
  public string? ProductName { get; set; }
}
