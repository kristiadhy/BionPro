using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class ProductStockConfiguration : IEntityTypeConfiguration<ProductStockModel>
{
  public void Configure(EntityTypeBuilder<ProductStockModel> builder)
  {
    builder.ToTable("M03ProductStock");

    builder.HasKey(c => c.StockId);
    builder.Property(c => c.StockId).ValueGeneratedOnAdd();
  }
}
