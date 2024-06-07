namespace Domain.Entities;
public class ProductLocationModel : BaseEntity
{
    public int LocationID { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
}
