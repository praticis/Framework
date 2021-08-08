
using System;
using System.Linq;

using Xunit;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class NotificationTests
    {
        [Fact]
        public void Notification_With_Message_Created_Successfully()
        {
            string msg = "Validation Message";
            var notification = new Notification(msg);

            Assert.NotEqual(default, notification.EventId);
            Assert.Null(notification.Code);
            Assert.Equal(msg, notification.Message);
            Assert.Equal(WorkType.Event, notification.WorkType);
            Assert.True(notification.IsValid);
            Assert.Empty(notification.Validate());
            Assert.False(notification.HasNotificationCode());
        }

        [Fact]
        public void Notification_With_Code_And_Message_Created_Successfully()
        {
            string code = "1", msg = "Notification Message";
            var notification = new Notification(code, msg);

            Assert.NotEqual(default, notification.EventId);
            Assert.Equal(code, notification.Code);
            Assert.Equal(msg, notification.Message);
            Assert.Equal(WorkType.Event, notification.WorkType);
            Assert.True(notification.IsValid);
            Assert.Empty(notification.Validate());
            Assert.True(notification.HasNotificationCode());
        }

        [Fact]
        public void Notification_With_NotificationId_And_Code_And_Value_Created_Successfully()
        {
            Guid notificationId = Guid.NewGuid();
            string code = "1", msg = "Notification Message";
            var notification = new Notification(notificationId, code, msg);

            Assert.Equal(notificationId, notification.EventId);
            Assert.Equal(code, notification.Code);
            Assert.Equal(msg, notification.Message);
            Assert.Equal(WorkType.Event, notification.WorkType);
            Assert.True(notification.IsValid);
            Assert.Empty(notification.Validate());
            Assert.True(notification.HasNotificationCode());
        }

        [Fact]
        public void Notification_Without_NotificationId_Is_Not_Valid()
        {
            var notification = new Notification(default, "1", "Notification Message");

            Assert.Equal(default, notification.EventId);
            Assert.False(notification.IsValid);
            Assert.Contains("The notification id is not defined.", notification.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Notification_Without_Message_Is_Not_Valid()
        {
            var notification = new Notification(null);

            Assert.Null(notification.Code);
            Assert.Null(notification.Message);
            Assert.False(notification.IsValid);
            Assert.Contains("The notification message can not be null or empty.", notification.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Notification_Without_Code_And_Message_Is_Not_Valid()
        {
            var notification = new Notification(null, null);

            Assert.Null(notification.Code);
            Assert.Null(notification.Message);
            Assert.False(notification.IsValid);
            Assert.Contains("The notification message can not be null or empty.", notification.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Notification_Without_NotificationId_And_Code_And_Message_Is_Not_Valid()
        {
            var notification = new Notification(default, null, null);

            Assert.Equal(default, notification.EventId);
            Assert.Null(notification.Code);
            Assert.Null(notification.Message);
            Assert.False(notification.IsValid);
            Assert.Contains("The notification id is not defined.", notification.Validate().Select(e => e.ErrorMessage));
            Assert.Contains("The notification message can not be null or empty.", notification.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Notification_ToString_Contains_Message_And_Code_Value_When_Is_Setted()
        {
            string code = "1", msg = "Notification Message";
            var notification = new Notification(code, msg);

            Assert.Contains(code, notification.ToString());
            Assert.Contains(msg, notification.ToString());
        }

        [Fact]
        public void Notification_ToString_With_Message_Value_When_No_Has_Code()
        {
            string msg = "Notification Message";
            var notification = new Notification(msg);

            Assert.Equal(msg, notification.ToString());
        }
    }
}