
using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Data.Write.EF;

using ProjectName.DomainName1.Domain.Interfaces.Repositories;
using ProjectName.DomainName1.Domain.Models;
using ProjectName.DomainName1.Infra.Server.Data.Context;

namespace ProjectName.DomainName1.Infra.Server.Data.Repositories
{
    public class CustomerRepository : BaseRepository<Customer>, ICustomerRepository
    {
        public CustomerRepository(DomainName1_Context context, ICustomerReadRepository  readRepository, IServiceBus serviceBus)
            : base(context, readRepository, serviceBus)
        {

        }
    }
}