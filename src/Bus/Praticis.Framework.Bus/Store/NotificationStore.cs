
using System;
using System.Linq;
using System.Collections.Generic;

using Praticis.Framework.Bus.Abstractions;

namespace Praticis.Framework.Bus.Store
{
    /// <summary>
    /// Store published warning, eystem error, log and domain notifications.
    /// </summary>
    public class NotificationStore : INotificationStore
    {
        private readonly List<Notification> _notifications;
        private readonly List<Warning> _warnings;
        private readonly List<SystemError> _systemErrors;
        private readonly List<Log> _logs;

        /// <summary>
        /// Create an execution context notification store
        /// </summary>
        /// <param name="notifications">The domain notification store.</param>
        /// <param name="warnings">The warning store.</param>
        /// <param name="systemErrors">The system error store.</param>
        /// <param name="logs">The log store.</param>
        public NotificationStore(List<Notification> notifications, List<Warning> warnings, 
            List<SystemError> systemErrors, List<Log> logs)
        {
            this._notifications = notifications;
            this._warnings = warnings;
            this._systemErrors = systemErrors;
            this._logs = logs;
        }

        /// <summary>
        /// Obtains the notification messages of the execution context.
        /// </summary>
        /// <returns>
        /// Returns a collection of domain notification messages.
        /// </returns>
        public IEnumerable<Notification> GetNotifications() => this._notifications;

        /// <summary>
        /// Obtains the system error messages of the execution context.
        /// </summary>
        /// <returns>
        /// Returns a collection of system error messages.
        /// </returns>
        public IEnumerable<SystemError> GetSystemErrors() => this._systemErrors;

        /// <summary>
        /// Obtains the warning messages of the execution context.
        /// </summary>
        /// <returns>
        /// Returns a collection of warning messages.
        /// </returns>
        public IEnumerable<Warning> GetWarnings() => this._warnings;

        /// <summary>
        /// Obtains the log messages of the execution context.
        /// </summary>
        /// <returns>
        /// Returns a collection of log messages.
        /// </returns>
        public IEnumerable<Log> GetLogs() => this._logs;

        /// <summary>
        /// Obtains all notification messages. 
        /// </summary>
        /// <param name="includeNotifications">Include notifications in return results.</param>
        /// <param name="includeWarnings">Include notifications in return results.</param>
        /// <param name="includeSystemErrors">Include notifications in return results.</param>
        /// <param name="includeLogs">Include notifications in return results.</param>
        /// <returns></returns>
        public IEnumerable<Notification> GetAll(bool includeNotifications = true, bool includeWarnings = true, bool includeSystemErrors = true, bool includeLogs = true)
        {
            List<Notification> collection = new List<Notification>();

            if (includeNotifications)
                collection.AddRange(this._notifications);

            if (includeWarnings)
                collection.AddRange(this._warnings);

            if (includeSystemErrors)
                collection.AddRange(this._systemErrors);

            if (includeLogs)
                collection.AddRange(this._logs);

            collection = collection.OrderBy(n => n.Time)
                .ToList();

            return collection;
        }

        /// <summary>
        /// Finde messages in notification store.
        /// </summary>
        /// <param name="predicate">Filters a sequence of notifications based on predicate value.</param>
        /// <param name="includeNotifications">Consider notifications in query.</param>
        /// <param name="includeWarnings">Consider warnings in query.</param>
        /// <param name="includeSystemErrors">Consider system errors in query.</param>
        /// <param name="includeLogs">Consider logs in query.</param>
        /// <returns></returns>
        public IEnumerable<Notification> Find(Func<Notification, bool> predicate, bool includeNotifications = true, bool includeWarnings = true, bool includeSystemErrors = true, bool includeLogs = true)
        {
            List<Notification> collection = new List<Notification>();

            if (includeNotifications)
                collection.AddRange(this._notifications.Where(predicate));

            if (includeWarnings)
                collection.AddRange(this._warnings.Where(predicate));

            if (includeSystemErrors)
                collection.AddRange(this._systemErrors.Where(predicate));

            if (includeLogs)
                collection.AddRange(this._logs.Where(predicate));

            collection = collection.OrderBy(n => n.Time)
                .ToList();

            return collection;
        }

        /// <summary>
        /// Verify if has messages in notification store. By default, verify notifications and system errors.
        /// Use the parameters to customise analysis filters.
        /// </summary>
        /// <param name="includeNotifications">Consider notifications to verify if has notifications.</param>
        /// <param name="includeWarnings">Consider warnings to verify if has notifications.</param>
        /// <param name="includeSystemErrors">Consider system errors to verify if has notifications.</param>
        /// <returns>
        /// Returns True if exists or False if do not exists.
        /// </returns>
        public bool HasNotifications(bool includeNotifications = true, bool includeWarnings = false, bool includeSystemErrors = true)
        {
            bool hasNotifications = false;

            if (includeNotifications && !hasNotifications)
                hasNotifications = this._notifications.Count > 0;

            if (includeWarnings && !hasNotifications)
                hasNotifications = this._warnings.Count > 0;

            if (includeSystemErrors && !hasNotifications)
                hasNotifications = this._systemErrors.Count > 0;

            return hasNotifications;
        }

        /// <summary>
        /// Verify if exists log messages in notification store.
        /// </summary>
        /// <returns>
        /// Returns True if exists or False to do not exists.
        /// </returns>
        public bool HasLogs() => this._logs.Count > 0;

        /// <summary>
        /// Verify if exists system error messages in notification store.
        /// </summary>
        /// <returns>
        /// Returns True if exists or False if do not exists.
        /// </returns>
        public bool HasSystemErrors() => this._systemErrors.Count > 0;

        /// <summary>
        /// Verify if has warning messages.
        /// </summary>
        /// <returns>
        /// Return True if exists or False if do not exists.
        /// </returns>
        public bool HasWarnings() => this._warnings.Count > 0;

        public void Clear()
        {
            this._notifications?.Clear();
            this._warnings?.Clear();
            this._systemErrors?.Clear();
            this._logs?.Clear();
        }
    }
}