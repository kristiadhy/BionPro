using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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