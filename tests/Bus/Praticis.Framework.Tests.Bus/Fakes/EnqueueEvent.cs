
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

using EventType = Praticis.Framework.Bus.Abstractions.Enums.NotificationType;

namespace Praticis.Framework.Tests.Bus.Fakes
{
    internal class EnqueueEvent : Event
    {
        public EnqueueEvent()
            : base(EventType.Default, ExecutionMode.Enqueue)
        {

        }

        public override IEnumerable<ValidationFailure> Validate()
            => new List<ValidationFailure>();
    }
}