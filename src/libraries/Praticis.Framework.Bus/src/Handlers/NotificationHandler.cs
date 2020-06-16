
using System;
using System.Collections.Generic;
using System.Threading;
using System.Threading.Tasks;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus.Handlers
{
    /// <summary>
    /// The notification store execution handler.
    /// </summary>
    public class NotificationHandler : IEventHandler<Notification>
    {
        private readonly List<Notification> _notifications;
        private readonly List<Warning> _warnings;
        private readonly List<SystemError> _systemErrors;
        private readonly List<Log> _logs;

        /// <summary>
        /// Create a notification store handler.
        /// </summary>
        /// <param name="provider"></param>
        public NotificationHandler(IServiceProvider provider)
        {
            this._notifications = provider.GetService<List<Notification>>();
            this._warnings = provider.GetService<List<Warning>>();
            this._systemErrors = provider.GetService<List<SystemError>>();
            this._logs = provider.GetService<List<Log>>();
        }
        
        public Task Handle(Notification notification, CancellationToken cancellationToken)
        {
            switch (notification.NotificationType)
            {
                case NotificationType.Domain_Notification:
                    this._notifications?.Add(notification);
                    break;

                case NotificationType.Warning:
                    this._warnings?.Add(notification as Warning);
                    break;

                case NotificationType.System_Error:
                    this._systemErrors?.Add(notification as SystemError);
                    break;

                case NotificationType.Log:
                    this._logs?.Add(notification as Log);
                    break;
            }

            return Task.CompletedTask;
        }

        /// <summary>
        /// Clear the notification store.
        /// </summary>
        public void Dispose()
        {
            this._notifications?.Clear();
            this._warnings?.Clear();
            this._systemErrors?.Clear();
            this._logs?.Clear();
        }
    }
}