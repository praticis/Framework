
using FluentValidation.Results;

using System.Collections.Generic;
using System.Runtime.CompilerServices;

namespace Praticis.Framework.Bus.Abstractions
{
    /// <summary>
    /// A log notification message.
    /// </summary>
    public class Log : Notification
    {
        #region Properties

        /// <summary>
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </summary>
        public string SourceMethod { get; private set; }

        /// <summary>
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </summary>
        public string SourceFileName { get; private set; }

        /// <summary>
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </summary>
        public int SourceLineNumber { get; private set; }

        /// <summary>
        /// The object data related to the log.
        /// </summary>
        public object ObjectManipulated { get; private set; }

        #endregion

        #region Constructors

        /// <summary>
        /// Create a log notification message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public Log(string message, [CallerMemberName] string sourceMethod = "", 
            [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFileName = "")
            : base(message, Enums.NotificationType.Log)
        {
            this.Message = message;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        /// <summary>
        /// Create a log notification message.
        /// </summary>
        /// <param name="message">The log message.</param>
        /// <param name="objectManipulated">The object data related to the log.</param>
        /// <param name="sourceMethod">
        /// The source method where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceLineNumber">
        /// The source line number where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        /// <param name="sourceFileName">
        /// The source file name where the log was registered.
        /// <strong>It is captured automatically</strong>.
        /// </param>
        public Log(string message, object objectManipulated, [CallerMemberName] string sourceMethod = "",
            [CallerLineNumber] int sourceLineNumber = 0, [CallerFilePath] string sourceFileName = "")
            : base(message, Enums.NotificationType.Log)
        {
            this.Message = message;
            this.ObjectManipulated = objectManipulated;
            this.SourceMethod = sourceMethod;
            this.SourceLineNumber = sourceLineNumber;
            this.SourceFileName = sourceFileName;
        }

        #endregion

        public override IEnumerable<ValidationFailure> Validate()
        {
            return base.Validate();
        }

        public override string ToString()
            => $"{this.Message} [Method: {this.SourceMethod}; Line: {this.SourceLineNumber}; File: {this.SourceFileName}]";
    }
}