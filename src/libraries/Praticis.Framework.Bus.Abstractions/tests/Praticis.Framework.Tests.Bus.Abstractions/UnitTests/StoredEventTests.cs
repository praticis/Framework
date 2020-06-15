
using System.Linq;

using Xunit;

using Praticis.Framework.Bus.Abstractions.Events;
using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Tests.Bus.Abstractions.Fakes;

namespace Praticis.Framework.Tests.Bus.Abstractions.UnitTests
{
    public class StoredEventTests
    {
        [Fact]
        public void StoredEvent_Created_Successfully()
        {
            var storedEvent = new DefaultEvent();
            var @event = new StoredEvent(storedEvent);
            
            Assert.NotEqual(default, @event.EventId);
            Assert.Equal(typeof(StoredEvent).Name, @event.EventName);
            Assert.NotEqual(default, @event.Time);
            Assert.Equal(NotificationType.Stored_Event, @event.NotificationType);
            Assert.Equal(ExecutionMode.Enqueue, @event.ExecutionMode);
            Assert.False(@event.ApplyEventSourcing);
            Assert.Equal(typeof(StoredEvent), @event.ResourceType);
            Assert.Equal(WorkType.Event, @event.WorkType);
            Assert.NotNull(@event.Event);
            Assert.True(@event.IsValid);
            Assert.Empty(@event.Validate());
        }

        [Fact]
        public void StoredEvent_Without_Event_Is_Not_Valid()
        {
            var @event = new StoredEvent(null);

            Assert.Null(@event.Event);
            Assert.False(@event.IsValid);
            Assert.Contains("The event to store can not be null.", @event.Validate().Select(e => e.ErrorMessage));
        }
    }
}