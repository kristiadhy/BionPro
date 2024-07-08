namespace Domain.DTO;
public class PurchaseDto
{
    public int PurchaseID { get; set; }
    public string? TransactionCode { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? Description { get; set; }
    public Guid? SupplierID { get; set; }
}

public class PurchaseDtoForQueries
{
    public int PurchaseID { get; set; }
    public string? TransactionCode { get; set; }
    public string? SupplierName { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? Description { get; set; }
    public decimal GrandTotal { get; set; }
}