
using System;

using MediatR;

namespace Praticis.Framework.Bus.Abstractions.Events
{
    /// <summary>
    /// The event execution abstraction implementation.
    /// </summary>
    /// <typeparam name="TEvent">The event that will carry the informations.</typeparam>
    public interface IEventHandler<TEvent> : INotificationHandler<TEvent>, IDisposable
        where TEvent : IEvent
    {

    }
}