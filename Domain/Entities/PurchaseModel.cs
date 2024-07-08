namespace Domain.Entities;
public class PurchaseModel : BaseEntity
{
    public int PurchaseID { get; set; }
    public string? TransactionCode { get; set; }
    public DateTimeOffset Date { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public string? Description { get; set; }

    public Guid? SupplierID { get; set; }
    public SupplierModel? Supplier { get; set; }
    public ICollection<PurchaseDetailModel>? PurchaseDetails { get; set; }
}
