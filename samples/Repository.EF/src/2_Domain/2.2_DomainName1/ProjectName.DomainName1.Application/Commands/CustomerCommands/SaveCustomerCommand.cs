
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;

using ProjectName.DomainName1.Application.ViewModels;

namespace ProjectName.DomainName1.Application.Commands.CustomerCommands
{
    public class SaveCustomerCommand : Command
    {
        public CustomerViewModel Customer { get; set; }
        
        public SaveCustomerCommand(CustomerViewModel customer)
        {
            this.Customer = customer;
        }

        public override IEnumerable<ValidationFailure> Validate()
        {
            return new List<ValidationFailure>();
        }
    }
}