using Domain.Entities;
using Microsoft.EntityFrameworkCore.Metadata.Builders;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
