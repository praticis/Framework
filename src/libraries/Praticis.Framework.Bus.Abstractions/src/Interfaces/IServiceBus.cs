
using System.Collections.Generic;
using System.Threading.Tasks;

using Praticis.Framework.Bus.Abstractions.Commands;
using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A wrapper implementation about MediatR that provide abstraction for event-driven programming.
    /// Make easy using the most message brokers, parallel worker process and more.
    /// </summary>
    public interface IServiceBus
    {
        /// <summary>
        /// Store published warning, eystem error, log and domain notifications.
        /// </summary>
        INotificationStore Notifications { get; }

        /// <summary>
        /// Send an event to be executed by <strong>all handlers</strong> signed to listening the publication event.
        /// If nothing is listening the event, no action will be executed.
        /// </summary>
        /// <param name="event">The event to send.</param>
        /// <returns>
        /// Events no has return in MediatR. The service bus returns notification store result value about if has or not has notifications.
        /// </returns>
        Task<bool> PublishEvent(IEvent @event);

        /// <summary>
        /// Send a command to be executed by <strong>one handler</strong>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>Returns <strong>True</strong> if successfully executed or <strong>False</strong> if failed.</returns>
        Task<bool> SendCommand(ICommand command);

        /// <summary>
        /// Send a command to be executed by <strong>one handler</strong>.
        /// </summary>
        /// <typeparam name="TResponse">The output response type of command.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>
        /// Returns the response of the command when ExecutionMode is <strong>WaitToClose</strong> or 
        /// default response value when ExecutionMode is <strong>Enqueue</strong>.
        /// </returns>
        Task<TResponse> SendCommand<TResponse>(ICommand<TResponse> command);

        /// <summary>
        /// Publish an EnqueueWorkEvent to a configured service bus extension enqueue
        /// it in a parallel work process stack.
        /// </summary>
        /// <param name="work">The work to enqueue.</param>
        /// <returns>
        /// Events no has return in MediatR. The service bus returns notification store
        /// result value about if has or not has notifications.
        /// </returns>
        Task<bool> EnqueueWork(IWork work);

        /// <summary>
        /// Publish an EnqueueWorkEvent to a configured service bus extension enqueue
        /// it in a parallel work process stack.
        /// </summary>
        /// <param name="works">The works to enqueue.</param>
        /// <returns>
        /// Events no has return in MediatR. The service bus returns notification store 
        /// result value about if has or not has notifications.
        /// </returns>
        Task<bool> EnqueueWork(IEnumerable<IWork> works);
    }
}