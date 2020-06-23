
using System;

using FluentValidation;

using ProjectName.DomainName1.Application.Commands;

namespace ProjectName.DomainName1.Application.Validations
{
    public class DefaultCommandValidation : AbstractValidator<DefaultCommand>
    {
        public DefaultCommandValidation()
        {
            RuleFor(r => r.SleepTime)
                .Equal(default(TimeSpan))
                    .WithMessage("Sleep time invalid.");
        }
    }
}