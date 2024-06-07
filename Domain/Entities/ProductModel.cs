namespace Domain.Entities;
public class ProductModel : BaseEntity
{
    public Guid ProductID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public int? CategoryID { get; set; }
    public string? ImageUrl { get; set; }

    public ProductCategoryModel Category { get; set; } = new();
    public ICollection<ProductPriceModel> Prices { get; set; } = [];
    public ICollection<ProductStockModel> Stocks { get; set; } = [];
}
