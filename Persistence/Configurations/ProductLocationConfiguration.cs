using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class ProductLocationConfiguration : IEntityTypeConfiguration<ProductLocationModel>
{
    public void Configure(EntityTypeBuilder<ProductLocationModel> builder)
    {
        builder.ToTable("M03ProductLocation");

        builder.HasKey(c => c.LocationID);
        builder.Property(c => c.LocationID).ValueGeneratedOnAdd();
        builder.Property(c => c.Name).IsRequired().HasMaxLength(200);
        builder.Property(c => c.Description).IsRequired().HasMaxLength(int.MaxValue);
        builder.HasMany(c => c.Stocks).WithOne(c => c.Location).HasForeignKey(c => c.LocationID).IsRequired(false);
    }
}
