
using System;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A warning notification message.
    /// </summary>
    public class Warning : Notification
    {
        #region Constructors

        /// <summary>
        /// Create a warning notification message.
        /// </summary>
        /// <param name="warningId">The warning id.</param>
        /// <param name="code">The warning code.</param>
        /// <param name="message">The warning message.</param>
        public Warning(Guid warningId, string code, string message)
            : base(warningId, code, message, Enums.NotificationType.Warning)
        {

        }

        /// <summary>
        /// Create a warning notification message.
        /// </summary>
        /// <param name="code">The warning code.</param>
        /// <param name="message">The warning message.</param>
        public Warning(string code, string message)
            : base(code, message, Enums.NotificationType.Warning)
        {

        }

        /// <summary>
        /// Create a warning notification message.
        /// </summary>
        /// <param name="message">The warning message.</param>
        public Warning(string message)
            : base(message, Enums.NotificationType.Warning)
        {

        }

        #endregion

        public override string ToString() => base.Message;
    }
}