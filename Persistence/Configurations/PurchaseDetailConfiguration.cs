using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class PurchaseDetailConfiguration : IEntityTypeConfiguration<PurchaseDetailModel>
{
    public void Configure(EntityTypeBuilder<PurchaseDetailModel> builder)
    {
        builder.ToTable("T01PurchaseDetails");

        builder.HasKey(e => new { e.ProductID, e.PurchaseID });
        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.DiscountPercentage).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");
    }
}