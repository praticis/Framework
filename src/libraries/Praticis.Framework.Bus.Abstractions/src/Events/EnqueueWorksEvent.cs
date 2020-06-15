
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Validations;

namespace Praticis.Framework.Bus.Abstractions.Events
{
    /// <summary>
    /// An event to enqueue works.
    /// </summary>
    public class EnqueueWorksEvent : Event
    {
        /// <summary>
        /// The works to enqueue.
        /// </summary>
        public IEnumerable<IWork> Works { get; private set; }

        /// <summary>
        /// Create an enqueue work event. The execution mode is WaitToClose by default.
        /// </summary>
        /// <param name="works">The works to enqueue.</param>
        public EnqueueWorksEvent(IEnumerable<IWork> works)
            : base(Enums.NotificationType.Work_Created, ExecutionMode.WaitToClose)
        {
            this.Works = works ?? new List<IWork>();
        }

        public override IEnumerable<ValidationFailure> Validate()
        {
            return new EnqueueWorksEventValidation().ValidateAsync(this)
                .GetAwaiter()
                .GetResult()
                .Errors;
        }
    }
}