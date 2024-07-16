namespace Domain.Entities;
public class ProductBarcodeModel : BaseEntity
{
    public string? EAN13Barcode { get; set; }
    public string? BarcodeImageUrl { get; set; }

    public Guid? ProductID { get; set; }
    public ProductModel? Product { get; set; }
}
