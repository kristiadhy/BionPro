using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class SaleConfiguration : IEntityTypeConfiguration<SaleModel>
{
  public void Configure(EntityTypeBuilder<SaleModel> builder)
  {
    builder.ToTable("T02Sales");

    builder.HasKey(e => e.SaleID);
    builder.Property(e => e.SaleID).ValueGeneratedOnAdd();

    builder.Property(e => e.TransactionCode).HasMaxLength(15);

    builder.Property(e => e.DiscountPercentage).HasColumnType("decimal(10, 2)");
    builder.Property(e => e.DiscountAmount).HasColumnType("decimal(10, 2)");

    builder.Property(e => e.Description).HasMaxLength(int.MaxValue);

    builder.HasMany(e => e.SaleDetails)
           .WithOne(e => e.Sale)
           .OnDelete(DeleteBehavior.Cascade);

    builder.HasOne(e => e.Customer)
        .WithMany(e => e.Sales)
        .HasForeignKey(e => e.CustomerID)
        .OnDelete(DeleteBehavior.Restrict);
  }
}