
using FluentValidation;

using ProjectName.DomainName1.Application.Commands;

namespace ProjectName.DomainName1.Application.Validations
{
    public class ResponseCommandValidation : AbstractValidator<ResponseCommand>
    {
        public ResponseCommandValidation()
        {
            RuleFor(r => r.Hello)
                .NotNull()
                    .WithMessage("Hello information can not be null.");
        }
    }
}