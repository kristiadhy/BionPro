using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class ProductConfiguration : IEntityTypeConfiguration<ProductModel>
{
    public void Configure(EntityTypeBuilder<ProductModel> builder)
    {
        builder.ToTable("M03Product");

        builder.HasKey(e => e.ProductID);
        builder.Property(c => c.ProductID).HasDefaultValueSql("NEWID()");

        builder.Property(e => e.Name)
            .IsRequired()
            .HasMaxLength(255);

        builder.Property(e => e.SKU)
            .IsRequired()
            .HasMaxLength(20);

        builder.Property(e => e.PriceAmount)
            .IsRequired()
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.Description)
            .IsRequired(false)
            .HasMaxLength(int.MaxValue);

        builder.Property(e => e.CategoryID)
            .IsRequired(false);

        builder.Property(e => e.ImageUrl)
            .IsRequired(false)
            .HasMaxLength(255);
    }
}