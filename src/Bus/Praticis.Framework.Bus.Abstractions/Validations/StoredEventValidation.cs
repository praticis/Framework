
using System;

using FluentValidation;

using Praticis.Framework.Bus.Abstractions.Events;

namespace Praticis.Framework.Bus.Abstractions.Validations
{
    internal class StoredEventValidation : AbstractValidator<StoredEvent>
    {
        public StoredEventValidation()
        {
            RuleFor(e => e.StoredEventId)
                .NotEqual(default(Guid))
                    .WithMessage("The store event id is not defined.");

            RuleFor(e => e.Event)
                .NotNull()
                    .WithMessage("The event to store can not be null.");
        }
    }
}