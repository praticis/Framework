
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

using EventType = Praticis.Framework.Bus.Abstractions.Enums.NotificationType;

namespace Praticis.Framework.Tests.Bus.Fakes
{
    internal class WaitToCloseEvent : Event
    {
        public WaitToCloseEvent()
            : base(EventType.Default, ExecutionMode.WaitToClose)
        {

        }

        public override IEnumerable<ValidationFailure> Validate()
            => new List<ValidationFailure>();
    }
}