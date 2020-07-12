
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Builders;

using ProjectName.DomainName1.Domain.Models;

namespace ProjectName.DomainName1.Infra.Server.Data.Mappings
{
    public class ContactMap : IEntityTypeConfiguration<Contact>
    {
        public void Configure(EntityTypeBuilder<Contact> builder)
        {
            builder.ToTable("Contacts");

            builder.HasKey(c => c.Id);

            builder.Property(c => c.Id)
                .HasColumnName("ContactId");

            builder.HasOne(c => c.Customer)
                .WithMany(c => c.Contacts)
                .HasForeignKey(c => c.CustomerId);
        }
    }
}