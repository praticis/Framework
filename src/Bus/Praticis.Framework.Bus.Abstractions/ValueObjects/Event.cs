
using System;
using System.Collections.Generic;
using System.Linq;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A generic execution order that can be executed only by many handlers.
    /// </summary>
    public abstract class Event : IEvent
    {
        #region Properties

        /// <summary>
        /// The Event Id.
        /// </summary>
        public Guid EventId { get; protected set; }

        /// <summary>
        /// The event name.
        /// </summary>
        public string EventName { get; }

        /// <summary>
        /// The time that the event occurred.
        /// </summary>
        public DateTime Time { get; protected set; }

        /// <summary>
        /// The event type represented by command.
        /// </summary>
        public int NotificationType { get; protected set; }

        /// <summary>
        /// The event execution mode.
        /// <strong>WaitToClose</strong> the event will be executed immediately.
        /// <strong>Enqueue</strong> the event will be enqueued in worker process and 
        /// executed after. <strong>Need to configure to use.</strong> 
        /// </summary>
        public ExecutionMode ExecutionMode { get; protected set; }

        /// <summary>
        /// Define if will be applied event sourcing to the event. Need configure to use.
        /// <see cref="https://martinfowler.com/eaaDev/EventSourcing.html"/>
        /// <seealso cref="https://microservices.io/patterns/data/event-sourcing.html"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing"/>
        /// Set <strong>True</strong> to apply or <strong>False</strong> to do not apply.
        /// </summary>
        public bool ApplyEventSourcing { get; protected set; }

        /// <summary>
        /// The assembly context of event.
        /// </summary>
        public Type ResourceType { get; set; }

        /// <summary>
        /// The work type of event.
        /// </summary>
        public WorkType WorkType => WorkType.Event;

        /// <summary>
        /// Verify if the event is valid. 
        /// Override <see cref="Validate"/> to implements validation.
        /// </summary>
        public bool IsValid => !this.Validate().Any();

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize an abstract event.
        /// </summary>
        /// <param name="notificationType">The notification type represented by the event.</param>
        /// <param name="executionMode">
        /// The event execution mode.
        /// <strong>WaitToClose</strong> the event will be executed immediately.
        /// <strong>Enqueue</strong> the event will be enqueued in worker process and 
        /// executed after. <strong>Need to configure to use.</strong> 
        /// </param>
        /// <param name="applyEventSourcing">
        /// Define if will be applied event sourcing to the event. Need configure to use.
        /// <see cref="https://martinfowler.com/eaaDev/EventSourcing.html"/>
        /// <seealso cref="https://microservices.io/patterns/data/event-sourcing.html"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing"/>
        /// Set <strong>True</strong> to apply or <strong>False</strong> to do not apply.
        /// </param>
        protected Event(int notificationType, ExecutionMode executionMode, bool applyEventSourcing = false)
        {
            this.EventId = Guid.NewGuid();
            this.Time = DateTime.Now;
            this.EventName = this.GetType().Name;
            this.ResourceType = this.GetType();
            this.NotificationType = notificationType;
            this.ExecutionMode = executionMode;
            this.ApplyEventSourcing = applyEventSourcing;
        }

        /// <summary>
        /// Initialize an abstract event.
        /// </summary>
        /// <param name="eventId">The event id.</param>
        /// <param name="notificationType">The event type.</param>
        /// <param name="executionMode">The event execution mode.</param>
        /// <param name="applyEventSourcing">
        /// Set <strong>True</strong> to store or <strong>False</strong> to do not store the event.
        /// </param>
        protected Event(Guid eventId, int notificationType, ExecutionMode executionMode, bool applyEventSourcing = false)
            : this(notificationType, executionMode, applyEventSourcing)
        {
            this.EventId = eventId;
        }

        #endregion

        /// <summary>
        /// Change the event execution mode.
        /// </summary>
        /// <param name="executionMode">The event execution mode.</param>
        public virtual void ChangeExecutionMode(ExecutionMode executionMode)
            => this.ExecutionMode = executionMode;

        /// <summary>
        /// Obtains the work id. Is the same value of event id.
        /// </summary>
        /// <returns>Returns the work id.</returns>
        public virtual Guid ObtainsWorkId() => this.EventId;

        /// <summary>
        /// Change the work id.
        /// </summary>
        /// <param name="id">The new work id.</param>
        public virtual void ChangeWorkId(Guid id) => this.EventId = id;

        /// <summary>
        /// Obtains the work name.
        /// </summary>
        /// <returns>Returns the work name.</returns>
        public virtual string ObtainsWorkName() => this.EventName;

        /// <summary>
        /// Execute event validation.
        /// </summary>
        /// <returns>
        /// Returns a validation failure collection if has validation messages or
        /// an empty list if not has messages.
        /// </returns>
        public abstract IEnumerable<ValidationFailure> Validate();
    }
}