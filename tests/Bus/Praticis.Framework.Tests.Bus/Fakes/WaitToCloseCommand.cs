
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;
using Praticis.Framework.Bus.Abstractions.Enums;

namespace Praticis.Framework.Tests.Bus.Fakes
{
    public class WaitToCloseCommand : Command
    {
        public WaitToCloseCommand()
            : base(ExecutionMode.WaitToClose)
        {

        }

        public override IEnumerable<ValidationFailure> Validate()
            => new List<ValidationFailure>();
    }
}