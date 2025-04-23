namespace Domain.DTO;
public class PurchaseDto
{
  public int? PurchaseID { get; set; }
  public string? TransactionCode { get; set; }
  public DateTimeOffset Date { get; set; }
  public decimal DiscountPercentage { get; set; }
  public decimal DiscountAmount { get; set; }
  public string? Description { get; set; }
  public Guid? SupplierID { get; set; }
  public string? SupplierName { get; set; }
  public List<PurchaseDetailDto> PurchaseDetails { get; set; } = [];
}

//It's an option. We use this to display purchase data with summary of the details without loading the details.
public class PurchaseDtoForSummary
{
  public int? PurchaseID { get; set; }
  public string? TransactionCode { get; set; }
  public Guid? SupplierID { get; set; }
  public string? SupplierName { get; set; }
  public DateTimeOffset Date { get; set; }
  public decimal DiscountPercentage { get; set; }
  public decimal DiscountAmount { get; set; }
  public string? Description { get; set; }
  public int TotalItems { get; set; }
  public int TotalQuantity { get; set; }
  public decimal GrandTotal { get; set; }
  public bool PaymentStatus { get; set; } = false;
}