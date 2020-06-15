
using System;
using System.Collections.Generic;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A notification store of the execution context.
    /// </summary>
    public interface INotificationStore : IDisposable
    {
        /// <summary>
        /// Obtains the domain notification messages stored during all execution process.
        /// Generally, if exists notifications the complete flow did not executed.
        /// </summary>
        /// <returns>
        /// Returns a collection of the notifications or an empty collection if don't exists.
        /// </returns>
        IEnumerable<Notification> GetNotifications();

        /// <summary>
        /// Obtains the warnings messages stored during all execution process.
        /// Generally, if not exists notifications and system errors, the complete flow
        /// has executed but have important observations to show the user.
        /// </summary>
        /// <returns>
        /// Returns a collection of the warnings or an empty collection if don't exists.
        /// </returns>
        IEnumerable<Warning> GetWarnings();

        /// <summary>
        /// Obtains the system errors messages stored during all execution process.
        /// If extists system errors, the complete flow do not executed.
        /// These messages can contains sensitive informations and can't show the user.
        /// </summary>
        /// <returns>
        /// Returns a collection of the system errors or an empty collection if don't exists.
        /// </returns>
        IEnumerable<SystemError> GetSystemErrors();

        /// <summary>
        /// Obtains the log messages stored during all execution process.
        /// </summary>
        /// <returns>
        /// Returns a collection of log messages or an empty collection if don't exists.
        /// </returns>
        IEnumerable<Log> GetLogs();

        /// <summary>
        /// Obtains all notifications ordered by time.
        /// </summary>
        /// <param name="includeNotifications">Set 'true' to include domain notifications.</param>
        /// <param name="includeWarnings">Set 'true' to include warnings notifications.</param>
        /// <param name="includeSystemErrors">Set 'true' to include system error notifications.</param>
        /// <param name="includeLogs">Set 'true' to include log messages.</param>
        /// <returns>
        /// Returns a collection of notifications or an empty collection if don't exists.
        /// </returns>
        IEnumerable<Notification> GetAll(bool includeNotifications = true, bool includeWarnings = true, bool includeSystemErrors = true, bool includeLogs = true);

        /// <summary>
        /// Filters a sequence of notifications based on a predicate ordered by time.
        /// </summary>
        /// <param name="predicate">A function to test each element for a condition.</param>
        /// <param name="includeNotifications">Set 'true' to include domain notifications.</param>
        /// <param name="includeWarnings">Set 'true' to include warnings notifications.</param>
        /// <param name="includeSystemErrors">Set 'true' to include system error notifications.</param>
        /// <param name="includeLogs">Set 'true' to include log messages.</param>
        /// <returns>
        /// Returns a collection of notifications or an empty collection if don't exists.
        /// </returns>
        IEnumerable<Notification> Find(Func<Notification, bool> predicate, bool includeNotifications = true, bool includeWarnings = true, bool includeSystemErrors = true, bool includeLogs = true);

        /// <summary>
        /// Verify if has notifications. By default consider notifications and system errors.
        /// Use the parameters to choose the options that be analisy.
        /// </summary>
        /// <param name="includeNotifications">Use 'True' to consider "notifications" in the analisy or 'False' to don't.</param>
        /// <param name="includeWarnings">Use 'True'to consider "warnings" in the analisy or 'False' to don't.</param>
        /// <param name="includeSystemErrors">Use 'True' to consider "system errors" in the analisy or 'False' to don't.</param>
        /// <returns>
        /// Return True if exists or False if do not exists.
        /// </returns>
        bool HasNotifications(bool includeNotifications = true, bool includeWarnings = false, bool includeSystemErrors = true);

        /// <summary>
        /// Verify if has log messages.
        /// </summary>
        /// <returns>
        /// Return True if exists or False if do not exists.
        /// </returns>
        bool HasLogs();

        /// <summary>
        /// Verify if has system error messages.
        /// </summary>
        /// <returns>
        /// Return True if exists or False if do not exists.
        /// </returns>
        bool HasSystemErrors();

        /// <summary>
        /// Verify if has warning messages.
        /// </summary>
        /// <returns>
        /// Return True if exists or False if do not exists.
        /// </returns>
        bool HasWarnings();
    }
}