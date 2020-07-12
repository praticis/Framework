
using System;
using System.Reflection;

using AutoMapper;
using Praticis.Extensions.Bus.Microsoft.DependencyInjection;

using ProjectName.DomainName1.Core.Handlers.CustomerHandlers;
using ProjectName.DomainName1.Infra.IoC;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProjectNameBootStrapper
    {
        public static void AddProjectNameModules(this IServiceCollection services)
        {
            services.AddServiceBus(new Assembly[] { typeof(SaveCustomerCommandHandler).Assembly });
            
            services.AddDomainName1Module();

            services.AddAutoMapper();
        }

        public static void AddAutoMapper(this IServiceCollection services)
        {
            services.AddAutoMapper(AppDomain.CurrentDomain.GetAssemblies());
        }
    }
}