
using FluentValidation;

using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus.Abstractions.Validations
{
    internal class EnqueueWorksEventValidation : AbstractValidator<EnqueueWorksEvent>
    {
        public EnqueueWorksEventValidation()
        {
            RuleFor(e => e.Works)
                .NotEmpty()
                    .WithMessage("Works can not be null or a empty collection.");
        }
    }
}