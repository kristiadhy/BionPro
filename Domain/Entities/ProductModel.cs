namespace Domain.Entities;
public class ProductModel : BaseEntity
{
    public Guid? ProductID { get; set; }
    public string? Name { get; set; }
    public string? SKU { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; } = true;

    public int? CategoryID { get; set; }
    public ProductCategoryModel? Category { get; set; }
    public ICollection<ProductStockModel>? Stocks { get; set; }
    public ICollection<PurchaseDetailModel>? PurchaseDetails { get; set; }
}
