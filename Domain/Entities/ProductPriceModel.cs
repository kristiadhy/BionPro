namespace Domain.Entities;
public class ProductPriceModel : BaseEntity
{
    public int PriceID { get; set; }
    public Guid ProductID { get; set; }
    public decimal PriceAmount { get; set; }
    public string? CurrencyCode { get; set; }
    public string? PriceType { get; set; }
    public DateTime? ValidFrom { get; set; }
    public DateTime? ValidTo { get; set; }

    public ProductModel Product { get; set; } = new();
}
