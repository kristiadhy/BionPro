using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class PurchaseConfiguration : IEntityTypeConfiguration<PurchaseModel>
{
    public void Configure(EntityTypeBuilder<PurchaseModel> builder)
    {
        builder.ToTable("T01Purchases");

        builder.HasKey(e => e.PurchaseID);
        builder.Property(e => e.PurchaseID).ValueGeneratedOnAdd();

        builder.Property(e => e.TransactionCode).HasMaxLength(15);

        builder.Property(e => e.DiscountPercentage).HasColumnType("decimal(10, 2)");
        builder.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");

        builder.Property(e => e.Description).HasMaxLength(int.MaxValue);

        builder.HasMany(e => e.PurchaseDetails)
               .WithOne(e => e.Purchase)
               .OnDelete(DeleteBehavior.Cascade);

        builder.HasOne(e => e.Supplier)
            .WithMany(e => e.Purchases)
            .HasForeignKey(e => e.SupplierID)
            .OnDelete(DeleteBehavior.Restrict);
    }
}