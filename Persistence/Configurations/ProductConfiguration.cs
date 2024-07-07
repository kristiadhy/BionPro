using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class ProductConfiguration : IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> builder)
    {
        builder.ToTable("M03Products");

        builder.HasKey(e => e.ProductID);
        builder.Property(c => c.ProductID).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Name).HasMaxLength(255);

        builder.Property(e => e.SKU).HasMaxLength(20);

        builder.Property(e => e.Price).HasColumnType("decimal(10, 2)");

        builder.Property(e => e.Description).HasMaxLength(int.MaxValue);

        //builder.HasOne(e => e.Category)
        //    .WithMany(e => e.Products)
        //    .HasForeignKey(e => e.CategoryID)
        //    .IsRequired(false);

        //builder.HasMany(e => e.Stocks)
        //    .WithOne(e => e.Product)
        //    .HasForeignKey(e => e.ProductID)
        //    .IsRequired(true);

        builder.Property(e => e.ImageUrl).HasMaxLength(500);
    }
}