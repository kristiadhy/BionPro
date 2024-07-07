namespace Domain.Entities;
public class PurchaseDetailModel
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubTotal { get; set; }

    public int? PurchaseID { get; set; }
    public PurchaseModel? Purchase { get; set; }
    public Guid ProductID { get; set; }
    public ProductModel? Product { get; set; }
}
