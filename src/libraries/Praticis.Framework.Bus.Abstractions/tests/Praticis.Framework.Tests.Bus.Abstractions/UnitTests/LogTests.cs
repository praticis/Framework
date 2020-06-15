
using System.Linq;

using Xunit;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class LogTests
    {
        [Fact]
        public void Log_With_Message_Created_Successfully()
        {
            string msg = "Log Message";
            var log = new Log(msg);

            Assert.NotEqual(default, log.EventId);
            Assert.Null(log.Code);
            Assert.Equal(msg, log.Message);
            Assert.NotNull(log.SourceMethod);
            Assert.NotNull(log.SourceFileName);
            Assert.True(log.SourceLineNumber >= 0);
            Assert.Null(log.ObjectManipulated);
            Assert.Equal(WorkType.Event, log.WorkType);
            Assert.True(log.IsValid);
            Assert.Empty(log.Validate());
        }

        [Fact]
        public void Log_With_Message_And_ObjectManipulated_Created_Successfully()
        {
            string msg = "Log Message";
            var obj = new DefaultCommand();

            var log = new Log(msg, obj);

            Assert.NotEqual(default, log.EventId);
            Assert.Null(log.Code);
            Assert.Equal(msg, log.Message);
            Assert.NotNull(log.SourceMethod);
            Assert.NotNull(log.SourceFileName);
            Assert.True(log.SourceLineNumber >= 0);
            Assert.Equal(obj, log.ObjectManipulated);
            Assert.Equal(WorkType.Event, log.WorkType);
            Assert.True(log.IsValid);
            Assert.Empty(log.Validate());
        }

        [Fact]
        public void Log_Without_Message_Is_Not_Valid()
        {
            var log = new Log(string.Empty);

            Assert.False(log.IsValid);
            Assert.Contains("The notification message can not be null or empty.", log.Validate().Select(e => e.ErrorMessage));

            log = new Log(string.Empty, null);

            Assert.False(log.IsValid);
            Assert.Contains("The notification message can not be null or empty.", log.Validate().Select(e => e.ErrorMessage));
        }

        [Fact]
        public void Log_ToString_Contains_Notification_Details()
        {
            string msg = "Log Message";
            var log = new Log(msg);

            Assert.Contains(msg, log.ToString());
            Assert.Contains(log.SourceMethod, log.ToString());
            Assert.Contains(log.SourceLineNumber.ToString(), log.ToString());
            Assert.Contains(log.SourceFileName, log.ToString());
        }
    }
}