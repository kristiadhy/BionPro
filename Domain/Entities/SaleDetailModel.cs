namespace Domain.Entities;
public class SaleDetailModel
{
    public int Quantity { get; set; }
    public decimal Price { get; set; }
    public decimal DiscountPercentage { get; set; }
    public decimal DiscountAmount { get; set; }
    public decimal SubTotal { get; set; }

    public int? SaleID { get; set; }
    public SaleModel? Sale { get; set; }
    public Guid? ProductID { get; set; }
    public ProductModel? Product { get; set; }
}
