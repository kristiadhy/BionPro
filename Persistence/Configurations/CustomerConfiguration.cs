using Domain.Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

namespace Persistence.Configurations;

internal sealed class CustomerConfiguration : IEntityTypeConfiguration<CustomerModel>
{
  public void Configure(EntityTypeBuilder<CustomerModel> builder)
  {
    builder.ToTable("M01Customers");

    builder.HasKey(c => c.CustomerID);
    builder.Property(c => c.CustomerID).HasDefaultValueSql("NEWID()");
    builder.Property(c => c.CustomerName).HasMaxLength(200);
    builder.Property(c => c.PhoneNumber).HasMaxLength(50);
    builder.Property(c => c.Email).HasMaxLength(100);
    builder.Property(c => c.ContactPerson).HasMaxLength(100);
    builder.Property(c => c.CPPhone).HasMaxLength(50);
  }
}
