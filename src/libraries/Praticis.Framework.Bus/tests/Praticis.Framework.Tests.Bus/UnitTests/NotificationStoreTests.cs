
using System.Collections.Generic;

using Xunit;

using Praticis.Framework.Bus.Store;
using Praticis.Framework.Bus.Abstractions;
using System.Linq;

namespace Praticis.Framework.Tests.Bus.UnitTests
{
    public class NotificationStoreTests
    {
        private List<Notification> _notifications { get; set; }
        private List<Warning> _warnings { get; set; }
        private List<SystemError> _systemErrors { get; set; }
        private List<Log> _logs { get; set; }

        public NotificationStoreTests()
        {
            this._notifications = new List<Notification>();
            this._warnings = new List<Warning>();
            this._systemErrors = new List<SystemError>();
            this._logs = new List<Log>();
        }

        [Fact]
        public void NotificationStore_When_Initialized_Then_No_Has_Notifications()
        {
            var store = new NotificationStore(this._notifications, this._warnings, this._systemErrors, this._logs);

            Assert.False(store.HasNotifications());
            Assert.Empty(store.GetAll());
        }

        [Fact]
        public void NotificationStore_When_Add_Notification_Then_HasNotifications()
        {
            var store = new NotificationStore(this._notifications, this._warnings, this._systemErrors, this._logs);
            var notification = new Notification("Notification Message");
            this._notifications.Add(notification);

            Assert.True(store.HasNotifications());
            Assert.False(store.HasWarnings());
            Assert.False(store.HasSystemErrors());
            Assert.False(store.HasLogs());

            Assert.Empty(store.GetSystemErrors());
            Assert.Empty(store.GetWarnings());
            Assert.Empty(store.GetLogs());

            Assert.Contains(notification, store.GetNotifications());
            Assert.Contains(notification, store.GetAll());
            Assert.Single(store.GetAll());
            Assert.Contains(notification, store.Find(n => n.Message == notification.Message));
        }

        [Fact]
        public void NotificationStore_When_Add_Warning_Then_HasWarnings()
        {
            var store = new NotificationStore(this._notifications, this._warnings, this._systemErrors, this._logs);
            var warning = new Warning("Warning Message");
            this._warnings.Add(warning);

            Assert.True(store.HasWarnings());
            Assert.False(store.HasNotifications());
            Assert.False(store.HasSystemErrors());
            Assert.False(store.HasLogs());

            Assert.Empty(store.GetSystemErrors());
            Assert.Empty(store.GetNotifications());
            Assert.Empty(store.GetLogs());

            Assert.Contains(warning, store.GetWarnings());
            Assert.Contains(warning, store.GetAll());
            Assert.Single(store.GetAll());
            Assert.Contains(warning, store.Find(n => n.Message == warning.Message));
        }

        [Fact]
        public void NotificationStore_When_Add_SystemError_Then_HasSystemErrors()
        {
            var store = new NotificationStore(this._notifications, this._warnings, this._systemErrors, this._logs);
            var error = new SystemError("Error Message");
            this._systemErrors.Add(error);

            Assert.True(store.HasSystemErrors());
            Assert.False(store.HasWarnings());
            Assert.True(store.HasNotifications());
            Assert.False(store.HasLogs());

            Assert.Empty(store.GetNotifications());
            Assert.Empty(store.GetWarnings());
            Assert.Empty(store.GetLogs());

            Assert.Contains(error, store.GetSystemErrors());
            Assert.Contains(error, store.GetAll());
            Assert.Single(store.GetAll());
            Assert.Contains(error, store.Find(n => n.Message == error.Message));
        }

        [Fact]
        public void NotificationStore_When_Add_Log_Then_HasLogs()
        {
            var store = new NotificationStore(this._notifications, this._warnings, this._systemErrors, this._logs);
            var log = new Log("Log Message");
            this._logs.Add(log);

            Assert.True(store.HasLogs());
            Assert.False(store.HasSystemErrors());
            Assert.False(store.HasWarnings());
            Assert.False(store.HasNotifications());

            Assert.NotEmpty(store.GetLogs());
            Assert.Empty(store.GetNotifications());
            Assert.Empty(store.GetWarnings());
            Assert.Empty(store.GetSystemErrors());

            Assert.Contains(log, store.GetLogs());
            Assert.Contains(log, store.GetAll());
            Assert.Single(store.GetAll());
            Assert.Contains(log, store.Find(n => n.Message == log.Message));
        }

        [Fact]
        public void NotificationStore_When_Add_Notification_And_Clear_Store_Then_Clean_Notifications()
        {
            var store = new NotificationStore(this._notifications, this._warnings, this._systemErrors, this._logs);

            this._notifications.Add(new Notification("Notification Message"));
            this._warnings.Add(new Warning("Warning Message"));
            this._systemErrors.Add(new SystemError("Error Message"));
            this._logs.Add(new Log("Log Message"));

            Assert.NotEmpty(store.GetAll());
            store.Clear();
            Assert.Empty(store.GetAll());
        }
    }
}