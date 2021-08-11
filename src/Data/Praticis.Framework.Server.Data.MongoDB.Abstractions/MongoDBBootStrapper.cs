
using System;

using Microsoft.Extensions.DependencyInjection;

using Praticis.Framework.Server.Data.MongoDB.Abstractions.Options;

namespace Praticis.Framework.Server.Data.MongoDB.Abstractions
{
    public static class MongoDBBootStrapper
    {
        public static void AddDbContext<TContext>(this IServiceCollection services, Action<DbContextOptions> options,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(contextLifetime);

            var op = new DbContextOptions();
            options.Invoke(op);

            switch (optionsLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(op);
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(provider => op);
                    break;

                default:
                    services.AddScoped(provider => op);
                    break;
            }
        }

        public static void AddDbContext<TContext>(this IServiceCollection services, Action<IServiceProvider, DbContextOptions> options,
            ServiceLifetime contextLifetime = ServiceLifetime.Scoped, ServiceLifetime optionsLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            services.AddDbContext<TContext>(contextLifetime);

            switch (optionsLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton(provider =>
                    {
                        var op = new DbContextOptions();
                        options.Invoke(provider, op);

                        return op;
                    });
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient(provider =>
                    {
                        var op = new DbContextOptions();
                        options.Invoke(provider, op);

                        return op;
                    });
                    break;

                default:
                    services.AddScoped(provider =>
                    {
                        var op = new DbContextOptions();
                        options.Invoke(provider, op);

                        return op;
                    });
                    break;
            }
        }

        private static void AddDbContext<TContext>(this IServiceCollection services, ServiceLifetime contextLifetime = ServiceLifetime.Scoped)
            where TContext : DbContext
        {
            switch (contextLifetime)
            {
                case ServiceLifetime.Singleton:
                    services.AddSingleton<TContext>();
                    break;

                case ServiceLifetime.Transient:
                    services.AddTransient<TContext>();
                    break;

                default:
                    services.AddScoped<TContext>();
                    break;
            }
        }
    }
}