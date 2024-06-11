using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;
internal sealed class SupplierConfiguration : IEntityTypeConfiguration<SupplierModel>
{
    public void Configure(EntityTypeBuilder<SupplierModel> builder)
    {
        builder.ToTable("M02Suppliers");

        builder.HasKey(c => c.SupplierID);
        builder.Property(c => c.SupplierID).HasDefaultValueSql("NEWID()");
        builder.Property(c => c.SupplierName).HasMaxLength(200);
        builder.Property(c => c.PhoneNumber).HasMaxLength(50);
        builder.Property(c => c.Email).HasMaxLength(100);
        builder.Property(c => c.ContactPerson).HasMaxLength(100);
        builder.Property(c => c.CPPhone).HasMaxLength(50);
    }
}
