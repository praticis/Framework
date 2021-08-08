
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Tests.Bus.Fakes
{
    internal class EnqueueResponseCommand : Command<string>
    {
        public EnqueueResponseCommand()
            : base(ExecutionMode.Enqueue)
        {

        }

        public override IEnumerable<ValidationFailure> Validate()
            => new List<ValidationFailure>();
    }
}
