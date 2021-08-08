
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Validations;

namespace Praticis.Framework.Bus.Abstractions.Events
{
    /// <summary>
    /// An event to enqueue work.
    /// </summary>
    public class EnqueueWorkEvent : Event
    {
        /// <summary>
        /// The work to enqueue.
        /// </summary>
        public IWork Work { get; private set; }

        /// <summary>
        /// Create an enqueue work event. The execution mode is WaitToClose by default.
        /// </summary>
        /// <param name="work">The work to enqueue.</param>
        public EnqueueWorkEvent(IWork work)
            : base(Enums.NotificationType.Work_Created, ExecutionMode.WaitToClose)
        {
            this.Work = work;
        }

        public override IEnumerable<ValidationFailure> Validate()
        {
            return new EnqueueWorkEventValidation().ValidateAsync(this)
                .GetAwaiter()
                .GetResult()
                .Errors;
        }
    }
}