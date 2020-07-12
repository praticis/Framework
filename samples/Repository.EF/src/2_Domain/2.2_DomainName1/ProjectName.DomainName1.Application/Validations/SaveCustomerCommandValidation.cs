
using System;

using FluentValidation;

using ProjectName.DomainName1.Application.Commands.CustomerCommands;

namespace ProjectName.DomainName1.Application.Validations
{
    public class SaveCustomerCommandValidation : AbstractValidator<SaveCustomerCommand>
    {
        public SaveCustomerCommandValidation()
        {
            
        }
    }
}