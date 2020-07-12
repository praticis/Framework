
using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Data.Read.EF;

using ProjectName.DomainName1.Domain.Interfaces.Repositories;
using ProjectName.DomainName1.Domain.Models;
using ProjectName.DomainName1.Infra.Server.Data.Context;

namespace ProjectName.DomainName1.Infra.Server.Data.Repositories
{
    public class CustomerReadRepository : BaseReadRepository<Customer>, ICustomerReadRepository
    {
        public CustomerReadRepository(DomainName1_Context context, IServiceBus serviceBus) 
            : base(context, serviceBus)
        {
        }
    }
}