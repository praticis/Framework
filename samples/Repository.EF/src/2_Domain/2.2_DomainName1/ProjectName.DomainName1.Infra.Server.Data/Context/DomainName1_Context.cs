
using Microsoft.EntityFrameworkCore;

using ProjectName.DomainName1.Infra.Server.Data.Mappings;

namespace ProjectName.DomainName1.Infra.Server.Data.Context
{
    public class DomainName1_Context : DbContext
    {
        public DomainName1_Context(DbContextOptions options)
            : base(options)
        {

        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.ApplyConfiguration(new CustomerMap());
            modelBuilder.ApplyConfiguration(new AddressMap());
            modelBuilder.ApplyConfiguration(new ContactMap());
        }
    }
}