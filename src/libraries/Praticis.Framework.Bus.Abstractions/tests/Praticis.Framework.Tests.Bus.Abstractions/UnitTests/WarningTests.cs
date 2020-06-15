
using System;
using System.Linq;

using Xunit;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class WarningTests
    {
        [Fact]
        public void Warning_With_Message_Created_Successfully()
        {
            string msg = "Warning Message";
            var warning = new Warning(msg);

            Assert.NotEqual(default, warning.EventId);
            Assert.Null(warning.Code);
            Assert.Equal(msg, warning.Message);
            Assert.Equal(WorkType.Event, warning.WorkType);
            Assert.True(warning.IsValid);
            Assert.Empty(warning.Validate());
            Assert.False(warning.HasNotificationCode());
        }

        [Fact]
        public void Warning_With_Code_And_Message_Created_Successfully()
        {
            string code = "1", msg = "Warning Message";
            var warning = new Warning(code, msg);

            Assert.NotEqual(default, warning.EventId);
            Assert.Equal(code, warning.Code);
            Assert.Equal(msg, warning.Message);
            Assert.Equal(WorkType.Event, warning.WorkType);
            Assert.True(warning.IsValid);
            Assert.Empty(warning.Validate());
            Assert.True(warning.HasNotificationCode());
        }

        [Fact]
        public void Warning_With_WarningId_And_Code_And_Value_Created_Successfully()
        {
            Guid WarningId = Guid.NewGuid();
            string code = "1", msg = "Warning Message";
            var warning = new Warning(WarningId, code, msg);

            Assert.Equal(WarningId, warning.EventId);
            Assert.Equal(code, warning.Code);
            Assert.Equal(msg, warning.Message);
            Assert.Equal(WorkType.Event, warning.WorkType);
            Assert.True(warning.IsValid);
            Assert.Empty(warning.Validate());
            Assert.True(warning.HasNotificationCode());
        }

        [Fact]
        public void Warning_Without_WarningId_Is_Not_Valid()
        {
            var Warning = new Warning(default, "1", "Warning Message");

            Assert.Equal(default, Warning.EventId);
            Assert.False(Warning.IsValid);
            Assert.Contains("The notification id is not defined.", Warning.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Warning_Without_Message_Is_Not_Valid()
        {
            var Warning = new Warning(null);

            Assert.Null(Warning.Code);
            Assert.Null(Warning.Message);
            Assert.False(Warning.IsValid);
            Assert.Contains("The notification message can not be null or empty.", Warning.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Warning_Without_Code_And_Message_Is_Not_Valid()
        {
            var Warning = new Warning(null, null);

            Assert.Null(Warning.Code);
            Assert.Null(Warning.Message);
            Assert.False(Warning.IsValid);
            Assert.Contains("The notification message can not be null or empty.", Warning.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Warning_Without_WarningId_And_Code_And_Message_Is_Not_Valid()
        {
            var Warning = new Warning(default, null, null);

            Assert.Equal(default, Warning.EventId);
            Assert.Null(Warning.Code);
            Assert.Null(Warning.Message);
            Assert.False(Warning.IsValid);
            Assert.Contains("The notification id is not defined.", Warning.Validate().Select(e => e.ErrorMessage));
            Assert.Contains("The notification message can not be null or empty.", Warning.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Warning_ToString_With_Message_Value_When_No_Has_Code()
        {
            string msg = "Warning Message";
            var Warning = new Warning(msg);

            Assert.Equal(msg, Warning.ToString());
        }
    }
}