
using System;
using System.Collections.Generic;

using Moq;
using Xunit;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Handlers;

namespace Praticis.Framework.Tests.Bus.UnitTests
{
    public class NotificationHandlerTests
    {
        private List<Notification> _notifications { get; set; }
        private List<Warning> _warnings { get; set; }
        private List<SystemError> _systemErrors { get; set; }
        private List<Log> _logs { get; set; }
        private Mock<IServiceProvider> _providerMock { get; set; }

        public NotificationHandlerTests()
        {
            this._providerMock = new Mock<IServiceProvider>();
            this._notifications = new List<Notification>();
            this._warnings = new List<Warning>();
            this._systemErrors = new List<SystemError>();
            this._logs = new List<Log>();

            this._providerMock.Setup(p => p.GetService(typeof(List<Notification>)))
                .Returns(this._notifications);

            this._providerMock.Setup(p => p.GetService(typeof(List<Warning>)))
                .Returns(this._warnings);

            this._providerMock.Setup(p => p.GetService(typeof(List<SystemError>)))
                .Returns(this._systemErrors);

            this._providerMock.Setup(p => p.GetService(typeof(List<Log>)))
                .Returns(this._logs);
        }

        [Fact]
        public void NotificationHandler_Add_Notification_Successfully()
        {
            var handler = new NotificationHandler(this._providerMock.Object);
            var notification = new Notification("Notification Message");

            handler.Handle(notification, default);

            Assert.Contains(notification, this._notifications);
            Assert.Empty(this._warnings);
            Assert.Empty(this._systemErrors);
            Assert.Empty(this._logs);
        }

        [Fact]
        public void NotificationHandler_Add_Warning_Successfully()
        {
            var handler = new NotificationHandler(this._providerMock.Object);
            var warning = new Warning("Warning Message");

            handler.Handle(warning, default);

            Assert.Contains(warning, this._warnings);
            Assert.Empty(this._notifications);
            Assert.Empty(this._systemErrors);
            Assert.Empty(this._logs);
        }

        [Fact]
        public void NotificationHandler_Add_SystemError_Successfully()
        {
            var handler = new NotificationHandler(this._providerMock.Object);
            var error = new SystemError("Error Message");

            handler.Handle(error, default);

            Assert.Contains(error, this._systemErrors);
            Assert.Empty(this._notifications);
            Assert.Empty(this._warnings);
            Assert.Empty(this._logs);
        }

        [Fact]
        public void NotificationHandler_Add_Log_Successfully()
        {
            var handler = new NotificationHandler(this._providerMock.Object);
            var log = new Log("Log Message");

            handler.Handle(log, default);

            Assert.Contains(log, this._logs);
            Assert.Empty(this._notifications);
            Assert.Empty(this._warnings);
            Assert.Empty(this._systemErrors);
        }
    }
}