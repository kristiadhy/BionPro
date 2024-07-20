using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class SaleDetailConfiguration : IEntityTypeConfiguration<SaleDetailModel>
{
    public void Configure(EntityTypeBuilder<SaleDetailModel> builder)
    {
        builder.ToTable("T02SaleDetails");

        builder.HasKey(e => new { e.ProductID, e.SaleID });
        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.DiscountPercentage).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.SubTotal).HasColumnType("decimal(10, 2)");
        //We forget to set the Sale Details on the Product model so by default, it is set to cascade delete. So, to prevent this, we set it here to prevent from the cascade delete.
        builder.HasOne(e => e.Product)
            .WithMany(e => e.SaleDetails)
            .HasForeignKey(e => e.ProductID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}