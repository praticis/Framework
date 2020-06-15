
using System;
using System.Collections.Generic;

using FluentValidation.Results;
using MediatR;

namespace Praticis.Framework.Bus.Abstractions.Events
{
    /// <summary>
    /// An application event abstraction.
    /// </summary>
    public interface IEvent : INotification, IWork
    {
        /// <summary>
        /// The event Id.
        /// </summary>
        Guid EventId { get; }

        /// <summary>
        /// The event name.
        /// </summary>
        string EventName { get; }

        /// <summary>
        /// The event time.
        /// </summary>
        DateTime Time { get; }

        /// <summary>
        /// Define if will be applied event sourcing to the event. Need configure to use.
        /// <see cref="https://martinfowler.com/eaaDev/EventSourcing.html"/>
        /// <seealso cref="https://microservices.io/patterns/data/event-sourcing.html"/>
        /// <seealso cref="https://docs.microsoft.com/en-us/azure/architecture/patterns/event-sourcing"/>
        /// Set <strong>True</strong> to apply or <strong>False</strong> to do not apply.
        /// </summary>
        bool ApplyEventSourcing { get; }

        /// <summary>
        /// The assembly context of the inherited event.
        /// </summary>
        Type ResourceType { get; }

        /// <summary>
        /// Verify if the event is valid. 
        /// Override <see cref="Validate"/> to implements validation.
        /// </summary>
        bool IsValid { get; }

        /// <summary>
        /// Execute event validation.
        /// </summary>
        /// <returns>
        /// Returns a validation failure collection if has validation messages or
        /// an empty list if not has messages.
        /// </returns>
        IEnumerable<ValidationFailure> Validate();
    }
}