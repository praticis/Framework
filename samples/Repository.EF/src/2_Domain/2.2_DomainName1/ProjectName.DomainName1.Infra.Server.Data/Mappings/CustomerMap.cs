
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectName.DomainName1.Domain.Models;

namespace ProjectName.DomainName1.Infra.Server.Data.Mappings
{
    public class CustomerMap : IEntityTypeConfiguration<Customer>
    {
        public void Configure(EntityTypeBuilder<Customer> builder)
        {
            builder.ToTable("Customers");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("CustomerId");
        }
    }
}