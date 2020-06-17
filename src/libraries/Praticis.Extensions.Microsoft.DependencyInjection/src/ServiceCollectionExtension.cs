
using System;
using System.Collections.Generic;
using System.Reflection;

using MediatR;
using Microsoft.Extensions.DependencyInjection;
using Praticis.Framework.Bus;
using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Handlers;
using Praticis.Framework.Bus.Store;

namespace Praticis.Extensions.Bus.Microsoft.DependencyInjection
{
    public static class ServiceCollectionExtension
    {
        /// <summary>
        /// Configure Praticis Framework in pipeline application
        /// Obtains the App Domain Assemblies and assign to MediatR.
        /// </summary>
        /// <param name="services">The service collection</param>
        public static void AddServiceBus(this IServiceCollection services)
        {
            // Add Mediator Service
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());

            // Service Bus Core
            services.AddScoped<IServiceBus, ServiceBus>();

            // Notification Store
            services.AddScoped<List<Log>>();
            services.AddScoped<List<Notification>>();
            services.AddScoped<List<Warning>>();
            services.AddScoped<List<SystemError>>();
            services.AddScoped<INotificationStore, NotificationStore>();

            // Handlers
            services.AddScoped<INotificationHandler<Log>, NotificationHandler>();
            services.AddScoped<INotificationHandler<Notification>, NotificationHandler>();
            services.AddScoped<INotificationHandler<Warning>, NotificationHandler>();
            services.AddScoped<INotificationHandler<SystemError>, NotificationHandler>();
        }

        /// <summary>
        /// Configure Praticis Framework in pipeline application.
        /// </summary>
        /// <param name="services">The service collection</param>
        /// <param name="assemblies">The Mediatr assemblies</param>
        public static void AddServiceBus(this IServiceCollection services, Assembly[] assemblies)
        {
            // Add Mediator Service
            services.AddMediatR(AppDomain.CurrentDomain.GetAssemblies());
            services.AddMediatR(assemblies);

            // Service Bus Core
            services.AddScoped<IServiceBus, ServiceBus>();

            // Notification Store
            services.AddScoped<List<Log>>();
            services.AddScoped<List<Notification>>();
            services.AddScoped<List<Warning>>();
            services.AddScoped<List<SystemError>>();
            services.AddScoped<INotificationStore, NotificationStore>();

            // Handlers
            services.AddScoped<INotificationHandler<Log>, NotificationHandler>();
            services.AddScoped<INotificationHandler<Notification>, NotificationHandler>();
            services.AddScoped<INotificationHandler<Warning>, NotificationHandler>();
            services.AddScoped<INotificationHandler<SystemError>, NotificationHandler>();
        }
    }
}