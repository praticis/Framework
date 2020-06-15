
using System;
using System.Collections.Generic;

using FluentValidation.Results;

using Praticis.Framework.Bus.Abstractions.Enums;
using Praticis.Framework.Bus.Abstractions.Validations;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A domain notification message.
    /// </summary>
    public class Notification : Event
    {
        #region Properties

        /// <summary>
        /// The notification code.
        /// </summary>
        public string Code { get; protected set; }

        /// <summary>
        /// The notification message.
        /// </summary>
        public string Message { get; protected set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a domain notification message.
        /// </summary>
        /// <param name="message">The notification message.</param>
        public Notification(string message)
            : base(Enums.NotificationType.Domain_Notification, ExecutionMode.WaitToClose)
        {
            this.Message = message;
        }

        /// <summary>
        /// Create a domain notification message.
        /// </summary>
        /// <param name="code">The notification code.</param>
        /// <param name="message">The notification message.</param>
        public Notification(string code, string message)
            : base(Enums.NotificationType.Domain_Notification, ExecutionMode.WaitToClose)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// Create a domain notification message.
        /// </summary>
        /// <param name="notificationId">The notification id.</param>
        /// <param name="code">The notification code.</param>
        /// <param name="message">The notification message.</param>
        public Notification(Guid notificationId, string code, string message)
            : base(notificationId, Enums.NotificationType.Domain_Notification, ExecutionMode.WaitToClose)
        {
            this.Code = code;
            this.Message = message;
        }

        /// <summary>
        /// Initializes a system notification message.
        /// </summary>
        /// <param name="message">The notification message.</param>
        /// <param name="notificationType">The notification type.</param>
        protected Notification(string message, int notificationType)
            : base(notificationType, ExecutionMode.WaitToClose)
        {
            this.Message = message;
            this.NotificationType = notificationType;
        }

        /// <summary>
        /// Create a domain notification message.
        /// </summary>
        /// <param name="code">The notification code.</param>
        /// <param name="message">The notification message.</param>
        /// <param name="notificationType">The notification type.</param>
        protected Notification(string code, string message, int notificationType)
            : base(notificationType, ExecutionMode.WaitToClose)
        {
            this.Code = code;
            this.Message = message;
            this.NotificationType = notificationType;
        }

        /// <summary>
        /// Create a domain notification message.
        /// </summary>
        /// <param name="notificationId">The notification id.</param>
        /// <param name="code">The notification code.</param>
        /// <param name="message">The notification value.</param>
        /// <param name="notificationType">The notification type.</param>
        protected Notification(Guid notificationId, string code, string message, int notificationType)
            : base(notificationId, notificationType, ExecutionMode.WaitToClose)
        {
            this.Code = code;
            this.Message = message;
            this.NotificationType = notificationType;
        }

        #endregion

        public bool HasNotificationCode() => !string.IsNullOrEmpty(this.Code);

        public override IEnumerable<ValidationFailure> Validate()
        {
            return new NotificationValidation().ValidateAsync(this)
                .GetAwaiter()
                .GetResult()
                .Errors;
        }

        public override string ToString()
        {
            if (string.IsNullOrEmpty(this.Code))
                return this.Message;

            return $"[{this.Code}] {this.Message}";
        }
    }
}