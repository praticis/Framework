
using System.Reflection;

using Praticis.Extensions.Bus.Microsoft.DependencyInjection;

using ProjectName.DomainName1.Core.Handlers;

namespace Microsoft.Extensions.DependencyInjection
{
    public static class ProjectNameBootStrapper
    {
        public static void AddProjectNameModules(this IServiceCollection services)
        {
            services.AddServiceBus(new Assembly[] { typeof(DefaultCommandHandler).Assembly });
        }
    }
}