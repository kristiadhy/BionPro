namespace Domain.Entities;
public class SaleModel : BaseEntity
{
  public int? SaleID { get; set; }
  public string? TransactionCode { get; set; }
  public DateTimeOffset Date { get; set; }
  public decimal DiscountPercentage { get; set; }
  public decimal DiscountAmount { get; set; }
  public string? Description { get; set; }

  public Guid? CustomerID { get; set; }
  public CustomerModel? Customer { get; set; }
  public ICollection<SaleDetailModel> SaleDetails { get; set; } = [];
}