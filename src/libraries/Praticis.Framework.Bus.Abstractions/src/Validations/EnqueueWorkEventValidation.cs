
using FluentValidation;

using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus.Abstractions.Validations
{
    internal class EnqueueWorkEventValidation : AbstractValidator<EnqueueWorkEvent>
    {
        public EnqueueWorkEventValidation()
        {
            RuleFor(e => e.Work)
                .NotNull()
                    .WithMessage("Work can not be null");
        }
    }
}