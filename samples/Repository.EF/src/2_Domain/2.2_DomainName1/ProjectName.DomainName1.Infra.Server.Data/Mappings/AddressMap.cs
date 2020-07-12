
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectName.DomainName1.Domain.Models;

namespace ProjectName.DomainName1.Infra.Server.Data.Mappings
{
    public class AddressMap : IEntityTypeConfiguration<Address>
    {
        public void Configure(EntityTypeBuilder<Address> builder)
        {
            builder.ToTable("Addresses");

            builder.Property(a => a.Id)
                .HasColumnName("AddressId");

            builder.HasOne(a => a.Customer)
                .WithOne(c => c.Address)
                .HasForeignKey<Address>(a => a.CustomerId)
                .HasConstraintName("FK_Customer_Address")
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}