using System.Collections.Generic;

using FluentValidation.Results;
using Praticis.Framework.Layers.Domain.Abstractions.Interfaces;


namespace Praticis.Framework.Layers.Domain.Abstractions.Objects
{
    /// <summary>
    /// An base object with basic equality and validation features based on Fluent Validation.
    /// </summary>
    public abstract class ValidatableObject : EquatableObject, IValidatable
    {
        private List<ValidationFailure> _notifications;
        /// <summary>
        /// The validation notification messages.
        /// </summary>
        public IEnumerable<ValidationFailure> Notifications { get; }

        /// <summary>
        /// Call <see cref="Validate"/> to obtains information 
        /// about validation status and returns <strong>true</strong> if is valid or 
        /// <strong>false</strong> if is not valid.
        /// </summary>
        public bool IsValid { get; }

        /// <summary>
        /// Execute the validate process.
        /// </summary>
        /// <returns>
        /// Returns <strong>true</strong> if is valid or
        /// <strong>false</strong> if is not valid.
        /// </returns>
        public abstract bool Validate();

        public ValidatableObject()
        { this._notifications = new List<ValidationFailure>(); }

        /// <summary>
        /// Add notification message in notification stack.
        /// </summary>
        /// <param name="notification">The notification messages</param>
        protected virtual void AddNotification(ValidationFailure notification)
            => this._notifications.Add(notification);

        /// <summary>
        /// Add notification collection message in notification stack.
        /// </summary>
        /// <param name="notification">The notification messages</param>
        protected virtual void AddNotifications(IEnumerable<ValidationFailure> notifications)
            => this._notifications.AddRange(notifications);

        /// <summary>
        /// Clear the notification message stack.
        /// </summary>
        protected virtual void ClearNotifications() => this._notifications.Clear();
    }
}
