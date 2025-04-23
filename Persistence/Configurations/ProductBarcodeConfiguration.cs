using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class ProductBarcodeConfiguration : IEntityTypeConfiguration<ProductBarcodeModel>
{
  public void Configure(EntityTypeBuilder<ProductBarcodeModel> builder)
  {
    builder.ToTable("M03ProductBarcodes");

    builder.HasNoKey(); //Because it's 1 on 1 relationship with the product table.
    builder.Property(e => e.EAN13Barcode).HasMaxLength(13);
    builder.Property(e => e.BarcodeImageUrl).HasMaxLength(500);
  }
}
