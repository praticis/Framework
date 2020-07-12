
using System;

using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

using ProjectName.DomainName1.Domain.Interfaces.Repositories;
using ProjectName.DomainName1.Infra.Server.Data.Context;
using ProjectName.DomainName1.Infra.Server.Data.Repositories;

namespace ProjectName.DomainName1.Infra.IoC
{
    public static class DomainName1BootStrapper
    {
        public static void AddDomainName1Module(this IServiceCollection services) 
        {
            // Repository

            services.AddDbContext<DomainName1_Context>((provider, op) =>
            {
                var config = provider.GetInstance<IConfiguration>();

                op.UseSqlServer(config.GetConnectionString("ProjectName_Connection"));
                op.EnableDetailedErrors();
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<ICustomerReadRepository, CustomerReadRepository>();
        }
    }
}