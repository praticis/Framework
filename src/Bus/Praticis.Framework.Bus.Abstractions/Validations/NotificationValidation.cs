
using FluentValidation;
using System;

namespace Praticis.Framework.Bus.Abstractions.Validations
{
    public class NotificationValidation : AbstractValidator<Notification>
    {
        public NotificationValidation()
        {
            RuleFor(n => n.EventId)
                .NotEqual(default(Guid))
                    .WithMessage("The notification id is not defined.");

            RuleFor(n => n.Message)
                .NotEmpty()
                    .WithMessage("The notification message can not be null or empty.");
        }
    }
}