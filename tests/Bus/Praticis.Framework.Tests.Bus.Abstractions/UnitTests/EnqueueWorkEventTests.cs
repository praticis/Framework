
using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Events;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;

using Xunit;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class EnqueueWorkEventTests
    {
        [Fact]
        public void EnqueueWorkEvent_Created_Successfully()
        {
            var cmd = new DefaultCommand();
            var @event = new EnqueueWorkEvent(cmd);

            Assert.Equal(cmd, @event.Work);
            Assert.Equal(NotificationType.Work_Created, @event.NotificationType);
            Assert.Equal(WorkType.Event, @event.WorkType);
            Assert.Equal(ExecutionMode.WaitToClose, @event.ExecutionMode);
            Assert.False(@event.ApplyEventSourcing);
            Assert.True(@event.IsValid);
            Assert.Empty(@event.Validate());
        }

        [Fact]
        public void EnqueueWorkEvent_Without_Work_Is_Not_Valid()
        {
            var @event = new EnqueueWorkEvent(null);

            Assert.False(@event.IsValid);
        }
    }
}