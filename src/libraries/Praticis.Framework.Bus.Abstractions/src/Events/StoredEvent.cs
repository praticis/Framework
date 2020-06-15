
using System;
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Validations;

using EventType = Praticis.Framework.Bus.Abstractions.Enums.NotificationType;

namespace Praticis.Framework.Bus.Abstractions.Events
{
    /// <summary>
    /// An event to store a notification message.
    /// </summary>
    public class StoredEvent : Event
    {
        #region Properties

        /// <summary>
        /// The stored event id. It is not event id.
        /// </summary>
        public Guid StoredEventId { get; private set; }

        /// <summary>
        /// The event to store.
        /// </summary>
        public IEvent Event { get; private set; }

        #endregion

        #region Costructors

        /// <summary>
        /// Create a stored event.
        /// </summary>
        /// <param name="event">The event to store.</param>
        public StoredEvent(IEvent @event)
            : base(EventType.Stored_Event, ExecutionMode.Enqueue, false)
        {
            this.StoredEventId = Guid.NewGuid();
            this.Event = @event;
        }

        #endregion

        public override IEnumerable<ValidationFailure> Validate()
        {
            return new StoredEventValidation().ValidateAsync(this)
                .GetAwaiter()
                .GetResult()
                .Errors;
        }
    }
}