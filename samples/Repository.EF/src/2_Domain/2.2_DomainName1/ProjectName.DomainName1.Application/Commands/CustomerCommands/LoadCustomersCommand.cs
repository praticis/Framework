
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;

using ProjectName.DomainName1.Application.ViewModels;

namespace ProjectName.DomainName1.Application.Commands.CustomerCommands
{
    public class LoadCustomersCommand : Command<IEnumerable<CustomerViewModel>>
    {
        public override IEnumerable<ValidationFailure> Validate()
            => new List<ValidationFailure>();
    }
}