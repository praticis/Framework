
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

using MediatR;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Commands;
using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus
{
    /// <summary>
    /// A wrapper implementation about MediatR that provide abstraction for event-driven programming.
    /// Make easy using the most message brokers, parallel worker process and more.
    /// </summary>
    public class ServiceBus : IServiceBus
    {
        private readonly IMediator _mediator;

        /// <summary>
        /// Store published warning, eystem error, log and domain notifications.
        /// </summary>
        public INotificationStore Notifications { get; }

        /// <summary>
        /// Create a service bus instance.
        /// </summary>
        /// <param name="mediator">The mediatR.</param>
        /// <param name="notificationStore">The notification store execution context.</param>
        public ServiceBus(IMediator mediator, INotificationStore notificationStore)
        {
            this._mediator = mediator;
            this.Notifications = notificationStore;
        }

        /// <summary>
        /// Send an event to be executed by <strong>all handlers</strong> 
        /// signed to listening the publication event.
        /// If nothing is listening the event, no action will be executed.
        /// </summary>
        /// <param name="event">The event to send.</param>
        /// <returns>
        /// Events no has return in MediatR. The service bus returns 
        /// notification store result value about if has or not has notifications.
        /// </returns>
        public virtual async Task<bool> PublishEvent(IEvent @event)
        {
            if (@event.ExecutionMode == ExecutionMode.WaitToClose)
                await this._mediator.Publish(@event);
            else
                await this.EnqueueWork(@event);

            return !this.Notifications.HasNotifications();
        }

        /// <summary>
        /// Send a command to be executed by <strong>one handler</strong>.
        /// </summary>
        /// <param name="command">The command to execute.</param>
        /// <returns>Returns <strong>True</strong> if successfully executed or <strong>False</strong> if failed.</returns>
        public virtual async Task<bool> SendCommand(ICommand command)
        {
            if (command.ExecutionMode == ExecutionMode.WaitToClose)
                return await this._mediator.Send(command);
            else
                return await this.EnqueueWork(command);
        }

        /// <summary>
        /// Send a command to be executed by <strong>one handler</strong>.
        /// </summary>
        /// <typeparam name="TResponse">The output response type of command.</typeparam>
        /// <param name="command">The command to execute.</param>
        /// <returns>
        /// Returns the response of the command when ExecutionMode is <strong>WaitToClose</strong> or 
        /// default response value when ExecutionMode is <strong>Enqueue</strong>.
        /// </returns>
        public virtual async Task<TResponse> SendCommand<TResponse>(ICommand<TResponse> command)
        {
            if (command.ExecutionMode == ExecutionMode.WaitToClose)
                return await this._mediator.Send(command);
            else
                await this.EnqueueWork(command);

            return default;
        }

        /// <summary>
        /// Publish an EnqueueWorkEvent to a configured service bus extension enqueue
        /// it in a parallel work process stack.
        /// </summary>
        /// <param name="work">The work to enqueue.</param>
        /// <returns>
        /// Events no has return in MediatR. The service bus returns notification store
        /// result value about if has or not has notifications.
        /// </returns>
        public virtual async Task<bool> EnqueueWork(IWork work)
        {
            work.ChangeExecutionMode(ExecutionMode.Enqueue);

            await this._mediator.Publish(new EnqueueWorkEvent(work));

            return !this.Notifications.HasNotifications();
        }

        /// <summary>
        /// Publish an EnqueueWorkEvent to a configured service bus extension enqueue
        /// it in a parallel work process stack.
        /// </summary>
        /// <param name="works">The works to enqueue.</param>
        /// <returns>
        /// Events no has return in MediatR. The service bus returns notification store 
        /// result value about if has or not has notifications.
        /// </returns>
        public virtual async Task<bool> EnqueueWork(IEnumerable<IWork> works)
        {
            foreach (var work in works.Where(w => w.ExecutionMode != ExecutionMode.Enqueue))
                work.ChangeExecutionMode(ExecutionMode.Enqueue);

            await this._mediator.Publish(new EnqueueWorksEvent(works));

            return !this.Notifications.HasNotifications();
        }
    }
}