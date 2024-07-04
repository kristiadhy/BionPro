namespace Domain.DTO;
public class ProductDtoForProductQueries
{
    public Guid ProductID { get; set; }
    public string? Name { get; set; }
    public string? SKU { get; set; }
    public decimal Price { get; set; }
    public string? Description { get; set; }
    public string? ImageUrl { get; set; }
    public bool IsActive { get; set; }
    public int? CategoryID { get; set; }
    public string? CategoryName { get; set; }
}
