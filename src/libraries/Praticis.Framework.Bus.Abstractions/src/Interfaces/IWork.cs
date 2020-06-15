
using System;

using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A Command or Event abstraction that can be enqueued.
    /// Command<TResponse> are not supported and can not be enqueued.
    /// </summary>
    public interface IWork
    {
        /// <summary>
        /// Obtains the work id. Is the same value of work or event.
        /// </summary>
        /// <returns>Returns the request id.</returns>
        Guid ObtainsWorkId();

        /// <summary>
        /// Obtains the work name.
        /// </summary>
        /// <returns>Returns the work name.</returns>
        string ObtainsWorkName();

        /// <summary>
        /// Change the work id.
        /// </summary>
        /// <param name="id">The new id.</param>
        void ChangeWorkId(Guid id);

        /// <summary>
        /// The event execution mode.
        /// <strong>WaitToClose</strong> the event will be executed immediately.
        /// <strong>Enqueue</strong> the event will be enqueued in worker process and 
        /// executed after. <strong>Need to configure to use.</strong> 
        /// </summary>
        ExecutionMode ExecutionMode { get; }
        
        /// <summary>
        /// The work type.
        /// </summary>
        WorkType WorkType { get; }

        /// <summary>
        /// The notification type represented by work.
        /// </summary>
        int NotificationType { get; }
        
        /// <summary>
        /// Change the work execution mode.
        /// </summary>
        /// <param name="executionMode">The work execution mode.</param>
        void ChangeExecutionMode(ExecutionMode executionMode);
    }
}