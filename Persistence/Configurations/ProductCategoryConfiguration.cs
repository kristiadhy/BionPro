using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal class ProductCategoryConfiguration : IEntityTypeConfiguration<ProductCategoryModel>
{
    public void Configure(EntityTypeBuilder<ProductCategoryModel> builder)
    {
        builder.ToTable("M03ProductCategories");

        builder.HasKey(c => c.CategoryID);
        builder.Property(c => c.CategoryID).ValueGeneratedOnAdd();
        builder.Property(c => c.Name).HasMaxLength(200);
    }
}
