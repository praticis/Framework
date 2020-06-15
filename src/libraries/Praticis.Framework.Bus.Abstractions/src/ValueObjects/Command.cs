
using System;
using System.Collections.Generic;
using System.Linq;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions.Commands;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A generic execution order that can be executed only by one handler.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class Command : Command<bool>, ICommand
    {
        #region Constructors

        /// <summary>
        /// Initialize an abstract command.
        /// </summary>
        /// <param name="executionMode">
        /// 'Wait To Close' mode will be executed imemdiately. 
        /// 'Queue' mode will be queued to execute in background.
        /// </param>
        /// <param name="notificationType">The event type represented by command.</param>
        protected Command(ExecutionMode executionMode = ExecutionMode.WaitToClose, int notificationType = Enums.NotificationType.Default)
        {
            this.CommandId = Guid.NewGuid();
            this.Time = DateTime.Now;
            this.ResourceType = this.GetType();
            this.CommandName = this.GetType().Name;
            this.ExecutionMode = executionMode;
            this.NotificationType = notificationType;
        }

        #endregion   
    }

    /// <summary>
    /// Represents an execution order that can be executed anywhere.
    /// </summary>
    /// <typeparam name="TResponse"></typeparam>
    public abstract class Command<TResponse> : ICommand<TResponse>
    {
        #region Properties

        /// <summary>
        /// The command id.
        /// </summary>
        public Guid CommandId { get; protected set; }

        /// <summary>
        /// The command name.
        /// </summary>
        public string CommandName { get; protected set; }

        /// <summary>
        /// The creation command time.
        /// </summary>
        public DateTime Time { get; protected set; }

        /// <summary>
        /// The work type of command.
        /// </summary>
        public WorkType WorkType => WorkType.Command;

        /// <summary>
        /// The event execution mode.
        /// <strong>WaitToClose</strong> the event will be executed immediately.
        /// <strong>Enqueue</strong> the event will be enqueued in worker process and 
        /// executed after. <strong>Need to configure to use.</strong> 
        /// </summary>
        public ExecutionMode ExecutionMode { get; protected set; }

        /// <summary>
        /// The event type represented by command.
        /// </summary>
        public int NotificationType { get; protected set; }

        /// <summary>
        /// The assembly context of the inherited command.
        /// </summary>
        public Type ResourceType { get; protected set; }

        /// <summary>
        /// Verify if the command is valid. 
        /// Override <see cref="Validate"/> to implements validation.
        /// </summary>
        public bool IsValid => !this.Validate().Any();

        #endregion

        #region Constructors

        /// <summary>
        /// Initialize an abstract command.
        /// </summary>
        /// <param name="executionMode">
        /// The event execution mode.
        /// <strong>WaitToClose</strong> the event will be executed immediately.
        /// <strong>Enqueue</strong> the event will be enqueued in worker process and 
        /// executed after. <strong>Need to configure to use.</strong> 
        /// </param>
        /// <param name="notificationType">The notification type represented by command.</param>
        protected Command(ExecutionMode executionMode = ExecutionMode.WaitToClose, int notificationType = Enums.NotificationType.Default)
        {
            this.CommandId = Guid.NewGuid();
            this.Time = DateTime.Now;
            this.ResourceType = this.GetType();
            this.CommandName = this.GetType().Name;
            this.ExecutionMode = executionMode;
            this.NotificationType = notificationType;
        }

        #endregion

        /// <summary>
        /// Change the command execution mode.
        /// </summary>
        /// <param name="executionMode">The command execution mode.</param>
        public virtual void ChangeExecutionMode(ExecutionMode executionMode)
            => this.ExecutionMode = executionMode;

        /// <summary>
        /// Obtains the work id. Is the same value of command id.
        /// </summary>
        /// <returns>Returns the work id.</returns>
        public virtual Guid ObtainsWorkId() => this.CommandId;

        /// <summary>
        /// Change the work id.
        /// </summary>
        /// <param name="id">The new work id.</param>
        public virtual void ChangeWorkId(Guid id) => this.CommandId = id;

        /// <summary>
        /// Obtains the work name.
        /// </summary>
        /// <returns>Returns the work name.</returns>
        public virtual string ObtainsWorkName() => this.CommandName;

        /// <summary>
        /// Execute command validation.
        /// </summary>
        /// <returns>
        /// Returns a validation failure collection if has validation messages or
        /// an empty list if not has messages.
        /// </returns>
        public abstract IEnumerable<ValidationFailure> Validate();
    }
}