using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Persistence.Configurations;
internal class ProductPriceConfiguration : IEntityTypeConfiguration<ProductPriceModel>
{
    public void Configure(EntityTypeBuilder<ProductPriceModel> builder)
    {
        builder.ToTable("M03ProductLocation");

        builder.HasKey(c => c.PriceID);
        builder.Property(c => c.PriceID).ValueGeneratedOnAdd();

        builder.Property(e => e.PriceAmount)
            .IsRequired()
            .HasColumnType("decimal(10, 2)");

        builder.Property(e => e.CurrencyCode)
            .HasMaxLength(3)
            .IsRequired(false);

        builder.Property(e => e.PriceType)
            .HasMaxLength(50)
            .IsRequired(false);

        builder.HasOne(d => d.Product)
            .WithMany(p => p.Prices)
            .HasForeignKey(d => d.ProductID)
            .OnDelete(DeleteBehavior.Cascade);

        builder.Property(e => e.ValidFrom)
            .IsRequired(false);

        builder.Property(e => e.ValidTo)
            .IsRequired(false);
    }
}
