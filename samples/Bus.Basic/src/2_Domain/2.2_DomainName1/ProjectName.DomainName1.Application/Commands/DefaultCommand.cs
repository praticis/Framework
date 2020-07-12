
using System;
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions;
using ProjectName.DomainName1.Application.Validations;

namespace ProjectName.DomainName1.Application.Commands
{
    public class DefaultCommand : Command
    {
        public TimeSpan SleepTime { get; private set; }


        public DefaultCommand(TimeSpan sleepTime)
            => this.SleepTime = sleepTime;

        public override IEnumerable<ValidationFailure> Validate()
            => new DefaultCommandValidation().Validate(this).Errors;
    }
}