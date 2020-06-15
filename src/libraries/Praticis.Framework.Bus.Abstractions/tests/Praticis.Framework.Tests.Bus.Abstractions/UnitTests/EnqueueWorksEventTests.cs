
using Xunit;

using Praticis.Framework.Bus.Abstractions.Events;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class EnqueueWorksEventTests
    {
        [Fact]
        public void EnqueueWorksEvent_Created_Successfully()
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
        public void EnqueueWorksEvent_Without_Works_Is_Not_Valid()
        {
            var @event = new EnqueueWorksEvent(null);

            Assert.False(@event.IsValid);
        }
    }
}