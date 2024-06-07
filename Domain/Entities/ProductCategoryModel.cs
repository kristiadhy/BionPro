namespace Domain.Entities;
public class ProductCategoryModel : BaseEntity
{
    public int CategoryID { get; set; }
    public string Name { get; set; } = string.Empty;
}
