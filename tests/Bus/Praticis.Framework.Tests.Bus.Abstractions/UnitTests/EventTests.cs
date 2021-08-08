
using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;

using EventType = Praticis.Framework.Bus.Abstractions.Enums.NotificationType;

using Xunit;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class EventTests
    {
        [Fact]
        public void Event_Created_Successfully()
        {
            var @event = new DefaultEvent();

            Assert.NotEqual(default, @event.EventId);
            Assert.Equal(typeof(DefaultEvent).Name, @event.EventName);
            Assert.NotEqual(default, @event.Time);
            Assert.Equal(EventType.Default, @event.NotificationType);
            Assert.Equal(ExecutionMode.WaitToClose, @event.ExecutionMode);
            Assert.False(@event.ApplyEventSourcing);
            Assert.Equal(typeof(DefaultEvent), @event.ResourceType);
            Assert.Equal(WorkType.Event, @event.WorkType);
            Assert.True(@event.IsValid);
            Assert.Empty(@event.Validate());
        }
    }
}