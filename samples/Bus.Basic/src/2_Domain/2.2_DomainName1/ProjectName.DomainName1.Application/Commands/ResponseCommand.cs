
using System.Collections.Generic;

using FluentValidation.Results;
using Praticis.Framework.Bus.Abstractions;

using ProjectName.DomainName1.Application.Validations;
using ProjectName.DomainName1.Application.ViewModels;

namespace ProjectName.DomainName1.Application.Commands
{
    public class ResponseCommand : Command<string>
    {
        public HelloViewModel Hello { get; private set; }

        public ResponseCommand(HelloViewModel hello)
            => this.Hello = hello;

        public ResponseCommand(string name)
            => this.Hello = new HelloViewModel { Name = name };

        public override IEnumerable<ValidationFailure> Validate()
            => new ResponseCommandValidation().ValidateAsync(this).GetAwaiter().GetResult().Errors;
    }
}