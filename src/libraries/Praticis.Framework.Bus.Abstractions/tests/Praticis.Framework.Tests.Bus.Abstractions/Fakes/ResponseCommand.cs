
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;

namespace Praticis.Framework.Tests.Bus.Abstractions.Fakes
{
    internal class ResponseCommand : Command<string>
    {
        public override IEnumerable<ValidationFailure> Validate()
            => new List<ValidationFailure>();
    }
}